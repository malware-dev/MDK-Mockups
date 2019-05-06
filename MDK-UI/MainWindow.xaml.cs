using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using IngameScript.Mockups;
using IngameScript.Mockups.Base;
using IngameScript.Mockups.Blocks;
using MDK_UI.MockupExtensions;
using Microsoft.Win32;
using Sandbox.ModAPI.Ingame;
using VRage.Game;

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static string UpdateUri { get; } = "https://github.com/malware-dev/MDK-SE/releases/latest";

        public string AssemblyName { get; private set; }
        public Version AssemblyVersion { get; }
        public string AssemblyAuthor { get; }

        public Type ProgramType { get; private set; }
        public MockCubeGrid Grid { get; private set; } = new MockCubeGrid();
        public MockGridTerminalSystem Terminal { get; set; } = new MockGridTerminalSystem();
        public ObservableCollection<IMyBlockGroup> Groups { get; }
        public ObservableCollection<IMyTerminalBlock> Blocks { get; }
        public ObservableCollection<IMyBlockGroup> BlockGroups { get; set; } = new ObservableCollection<IMyBlockGroup>();

        public IMyTerminalBlock SelectedBlock => lbGridComponents.SelectedItem as IMyTerminalBlock;
        public IEnumerable<IMyProgrammableBlock> ProgrammableBlocks => Terminal.OfType<IMyProgrammableBlock>();
        public IMyProgrammableBlock ProgrammableBlock { get; private set; }
        public string Log { get; private set; }

        public int TargetTickRate
        {
            get { return Thread.VolatileRead(ref _targetTickRate); }
            set { Interlocked.Exchange(ref _targetTickRate, value); }
        }
        public int TickCount => Thread.VolatileRead(ref _tickCount);
        public double TickRate => Thread.VolatileRead(ref _tickRate) / RuntimeConstants.TicksPerSecond;

        private int _targetTickRate = RuntimeConstants.TicksPerSecond;
        private int _tickCount = 1;
        private double _tickRate = 0;
        private readonly StringBuilder _log = new StringBuilder();

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object LogLock = new object();
        private UiMockedRun Runtime { get; set; }
        private Task RuntimeTask { get; set; }
        private bool AutoScroll { get; set; } = true;

        private CancellationTokenSource SimulationCancelled { get; set; } = new CancellationTokenSource();
        private ManualResetEvent SimulationRunning { get; } = new ManualResetEvent(true);

        private bool IsChanged { get; set; } = false;

        public MainWindow(string filename)
        {
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            AssemblyName = info.FileDescription;
            AssemblyVersion = new Version(info.ProductVersion);
            AssemblyAuthor = info.CompanyName;

            Groups = new ObservableCollection<IMyBlockGroup>(Terminal.Groups);
            Groups.CollectionChanged += Groups_CollectionChanged;

            Blocks = new ObservableCollection<IMyTerminalBlock>(Terminal.Blocks);
            Blocks.CollectionChanged += Blocks_CollectionChanged;

            Runtime = new UiMockedRun(WriteLog, Terminal);

            InitializeComponent();

            DataContext = this;

            if (!string.IsNullOrWhiteSpace(filename))
                LoadProgramFromAssembly(filename);
        }

        private void LoadProgramFromAssembly(string filename = "")
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                var file = new OpenFileDialog()
                {
                    Filter = "MDK-SE Executables|*.exe",
                    Multiselect = false,
                    InitialDirectory = Environment.CurrentDirectory
                };

                if (file.ShowDialog() == true)
                {
                    filename = file.FileName;
                }
                else
                {
                    return;
                }
            }

            if (!Path.IsPathRooted(filename))
            {
                filename = Path.Combine(Environment.CurrentDirectory, filename);
            }

            ProgramType = null;
            try
            {
                Assembly.LoadFile(Path.Combine(Path.GetDirectoryName(filename), "MDKUtilities.dll"));
                var assembly = Assembly.LoadFile(filename);

                var apiType = Type.GetType("Sandbox.ModAPI.IMyGridProgram, Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

                if (apiType == null)
                {
                    MessageBox.Show("Unable to load assembly.\nIt appears the game version has changed.", "Invalid argument", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(-1);
                }

                var programTypes = assembly.GetTypes().Where(t => !t.IsInterface && apiType.IsAssignableFrom(t));

                switch (programTypes.Count())
                {
                    case 0:
                        MessageBox.Show("Unable to find a compiled PB script.\nPlease make sure your program implements Sandbox.ModAPI.IMyGridProgram.", "Invalid argument", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case 1:
                        ProgramType = programTypes.ElementAt(0);
                        break;
                    default:
                        MessageBox.Show("Multiple PB scripts were detected.\nThe emulator only supports one script at at time.", "Invalid argument", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The provided assembly does not exist.", "Missing argument", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-2);
            }
            catch (BadImageFormatException)
            {
                MessageBox.Show("Unable to load assembly.\nIt appears to be the wrong format.", "Invalid argument", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-3);
            }

            if (ProgramType == null)
            {
                miReload.IsEnabled = false;
                btStartSimulation.IsEnabled = false;
                btStepSimulation.IsEnabled = false;

                if (Runtime != null)
                {
                    ResetSimulation();
                }
            }
            else
            {
                Blocks.Remove(ProgrammableBlock);

                ProgrammableBlock = new MockProgrammableBlock()
                {
                    CustomName = "Default PB",
                    CubeGrid = Grid,
                    ProgramType = ProgramType
                };

                Blocks.Add(ProgrammableBlock);

                miReload.IsEnabled = true;
                btStartSimulation.IsEnabled = true;
                btStepSimulation.IsEnabled = true;
                ResetSimulation();
            }
        }

        private void WriteLog(string text) 
            => _log.AppendLine(text);

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        private void ResetSimulation()
        {
            SimulationRunning.Reset();
            SimulationCancelled.Cancel();

            SimulationCancelled = new CancellationTokenSource();

            RuntimeTask = Task.Factory.StartNew(async () =>
            {
                Interlocked.Exchange(ref _tickCount, 1);

                var timer = Stopwatch.StartNew();
                var currentTargetTickRate = 0;
                var currentTicks = 0;
                
                while (!SimulationCancelled.IsCancellationRequested)
                {
                    SimulationRunning.WaitOne();

                    var targetTickRate = Thread.VolatileRead(ref _targetTickRate);
                    var tickrate = 1000d / targetTickRate;

                    if (targetTickRate != currentTargetTickRate)
                    {
                        currentTargetTickRate = targetTickRate;
                        currentTicks = 0;
                        timer.Restart();
                    }

                    var last = timer.ElapsedMilliseconds;

                    TickOnce();

                    var duration = timer.ElapsedMilliseconds - last;
                    var tickdelay = tickrate - duration;

                    if (tickdelay < tickrate)
                        tickdelay = tickrate;

                    currentTicks++;
                    Interlocked.Exchange(ref _tickRate, currentTicks / timer.Elapsed.TotalSeconds);
                    OnPropertyChanged(nameof(TickRate));

                    try
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(tickdelay), SimulationCancelled.Token);
                    }
                    catch (TaskCanceledException) { }
                }
            });
        }

        private void TickOnce()
        {
            _log.Clear();

            foreach (var block in Blocks.OfType<IMockupRuntimeProvider>().OrderBy(b => b.ProcessPriority))
            {
                block.ProcessGameTick(Terminal, _tickCount);
            }

            Runtime.NextTick();

            Interlocked.Increment(ref _tickCount);
            lock (LogLock)
            {
                Log = (Log += Environment.NewLine + _log.ToString()).Trim();
            }

            OnPropertyChanged(nameof(Log));
            OnPropertyChanged(nameof(TickCount));
        }

        private void Groups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => MapCollectionChangeTo(e, Terminal.Groups);

        private void Blocks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => MapCollectionChangeTo(e, Terminal.Blocks);

        private void MapCollectionChangeTo<T>(NotifyCollectionChangedEventArgs e, List<T> target)
        {
            var newItems = e.NewItems?.OfType<T>();
            var oldItems = e.OldItems?.OfType<T>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    target.AddRange(newItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    target.RemoveAll(g => oldItems.Contains(g));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    target.Clear();
                    break;
            }
        }

        private void MiMdk_Click(object sender, RoutedEventArgs e)
            => Process.Start("https://github.com/malware-dev/MDK-SE");

        private void MiAbout_Click(object sender, RoutedEventArgs e)
            => MessageBox.Show($"v{AssemblyVersion}\nCreated by {AssemblyAuthor}\nSpecial Thanks to malware-dev", $"About {AssemblyName}", MessageBoxButton.OK, MessageBoxImage.Information);

        private void MiExit_Click(object sender, RoutedEventArgs e)
        {
            if (AbandonChanges())
                Environment.Exit(0);
        }

        private async void MiUpdate_Click(object sender, RoutedEventArgs e)
        {
            var latest = default(Version);
            
            var request = (HttpWebRequest) WebRequest.Create(UpdateUri);
            request.Method = "HEAD";

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Version.TryParse(response.ResponseUri.LocalPath.Split('/').Last(), out latest);
                }
            }

            if (latest == null || (latest.Major == 0 && latest.Minor == 0))
            {
                MessageBox.Show("Unable to check for a newer version.", "Check for Update", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (latest > AssemblyVersion)
            {
                var download = MessageBox.Show($"A new version is available, would you like to download it?", "Check for Update", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (download == MessageBoxResult.Yes && AbandonChanges())
                {
                    Process.Start(UpdateUri);
                    Environment.Exit(0);
                }
            }
        }

        private bool AbandonChanges()
        {
            if (IsChanged)
            {
                var abandon = MessageBox.Show("You have unsaved changes, discard?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (abandon == MessageBoxResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private void BtAddGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddGroupDialogBox()
            {
                Owner = this
            };
            dialog.OnSubmit += AddGroup_OnSubmit;
            dialog.ShowDialog();
        }

        private void AddGroup_OnSubmit(object sender, string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Groups.Add(new MockBlockGroup(title));
            }
        }

        private void BtRemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (lbBlockGroups.SelectedItem is IMyBlockGroup selected)
            {
                Groups.Remove(selected);
                BlockGroups.Remove(selected);
            }
        }

        private void BtAddComponent_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddBlockDialogBox()
            {
                Owner = this
            };
            dialog.OnSubmit += AddBlock_OnSubmit;
            dialog.ShowDialog();
        }

        private void AddBlock_OnSubmit(object sender, string title, Type type)
        {
            if (!string.IsNullOrEmpty(title) && type != null)
            {
                var item = (MockTerminalBlock) Activator.CreateInstance(type);
                item.CustomName = title;
                item.CubeGrid = Grid;

                Blocks.Add(item);
                lbGridComponents.SelectedItem = item;
            }
        }

        private void BtRemoveComponent_Click(object sender, RoutedEventArgs e)
        {
            if(lbGridComponents.SelectedItem is IMyTerminalBlock selected)
            {
                if (selected != ProgrammableBlocks.First())
                {
                    Blocks.Remove(selected);
                }
                else
                {
                    MessageBox.Show("You cannot remove this PB.", "Invalid action", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CbGridSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var control = sender as ComboBox;
            var value = (MyCubeSize) control?.SelectedItem;

            if (value == MyCubeSize.Large && cbGridStatic != null)
            {
                Grid.IsStatic = cbGridStatic.IsChecked ?? false;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void BtStartSimulation_Click(object sender, RoutedEventArgs e)
        {
            SimulationRunning.Set();
            btStepSimulation.IsEnabled = false;
            btStartSimulation.IsEnabled = false;

            btStopSimulation.IsEnabled = true;
            btResetSimulation.IsEnabled = true;
        }

        private void BtStopSimulation_Click(object sender, RoutedEventArgs e)
        {
            SimulationRunning.Reset();

            btStepSimulation.IsEnabled = true;
            btStartSimulation.IsEnabled = true;

            btStopSimulation.IsEnabled = false;
            btResetSimulation.IsEnabled = true;
        }

        private void BtStepSimulation_Click(object sender, RoutedEventArgs e)
        {
            btStepSimulation.IsEnabled = false;
            btStartSimulation.IsEnabled = false;
            btResetSimulation.IsEnabled = false;

            TickOnce();

            btStepSimulation.IsEnabled = true;
            btStartSimulation.IsEnabled = true;
            btResetSimulation.IsEnabled = true;
        }

        private void BtResetSimulation_Click(object sender, RoutedEventArgs e)
        {
            btStepSimulation.IsEnabled = false;
            btStartSimulation.IsEnabled = false;
            btResetSimulation.IsEnabled = false;
            btStopSimulation.IsEnabled = false;

            SimulationCancelled.Cancel();
            SimulationRunning.Set();
            RuntimeTask.Wait();

            ResetSimulation();

            Log = "";
            OnPropertyChanged(nameof(Log));
            
            btStepSimulation.IsEnabled = true;
            btStartSimulation.IsEnabled = true;
        }

        private void SvDebugOut_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var target = sender as ScrollViewer;
            if (e.ExtentHeightChange == 0)
            {
                if (target.VerticalOffset == target.ScrollableHeight)
                {
                    AutoScroll = true;
                }
                else
                {
                    AutoScroll = false;
                }
            }

            if (AutoScroll && e.ExtentHeightChange != 0)
            {
                target.ScrollToVerticalOffset(target.ExtentHeight);
            }
        }

        private void MiReload_Click(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetAssembly(ProgramType);
            var location = new Uri(assembly.CodeBase).LocalPath;
            LoadProgramFromAssembly(location);
        }

        private void MiLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadProgramFromAssembly();
        }

        private void LbGridComponents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BlockGroups.Clear();

            foreach (var group in Groups.OfType<MockBlockGroup>().Where(g => g.Blocks.Contains(SelectedBlock)))
            {
                BlockGroups.Add(group);
            }
        }

        private void BtAddBlockToGroup_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBlock == null)
                return;

            if (lbBlockGroups.SelectedValue is MockBlockGroup group)
            {
                if (!BlockGroups.Contains(group))
                {
                    BlockGroups.Add(group);
                }

                if (!group.Blocks.Contains(SelectedBlock))
                {
                    group.Blocks.Add(SelectedBlock);
                }
            }
        }

        private void BtRemoveBlockFromGroup_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBlock == null)
                return;

            if (lbBlockGroups.SelectedValue is MockBlockGroup group)
            {
                if (BlockGroups.Contains(group))
                {
                    BlockGroups.Remove(group);
                }

                if (group.Blocks.Contains(SelectedBlock))
                {
                    group.Blocks.Remove(SelectedBlock);
                }
            }
        }
    }
}
