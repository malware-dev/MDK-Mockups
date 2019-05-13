using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using IngameScript.Mockups;
using MDK_UI.Blueprints;
using Sandbox.ModAPI.Ingame;

namespace MDK_UI
{
    /// <summary>
    /// Interaction logic for AddBlockDialogBox.xaml
    /// </summary>
    public partial class ImportBlueprintDialogBox : Window
    {
        public enum ImportResult
        {
            Invalid,
            Failed,
            Aborted,
            Partial,
            Success
        }

        public delegate void CompleteEventHandler(object sender, ImportResult result);
        public event CompleteEventHandler OnComplete;

        private BlueprintParser Parser { get; } = new BlueprintParser();
        private string Filename { get; }

        public ImportBlueprintDialogBox(string filename, MockCubeGrid grid, ICollection<IMyBlockGroup> groups, ICollection<IMyTerminalBlock> blocks)
        {
            InitializeComponent();

            Filename = filename;

            Parser.Progress += (_, args) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    Progress.Maximum = args.Total;
                    Progress.Value = args.Current;
                }, DispatcherPriority.Normal);
            };

            Parser.Completed += (_, blueprint) =>
            {
                if (blueprint == null)
                {
                    Close(ImportResult.Failed);
                    return;
                }

                grid.CustomName = blueprint.Name;

                grid.GridSizeEnum = blueprint.Size;
                grid.IsStatic = blueprint.IsStatic;

                groups.Clear();
                blocks.Clear();

                blueprint.Blocks.ForEach(b => blocks.Add(b));
                blueprint.Groups.ForEach(g => groups.Add(g));

                if (blueprint.UnsupportedBlocks)
                {
                    Close(ImportResult.Partial);
                }
                else
                {
                    Close(ImportResult.Success);
                }
            };
        }

        public new void ShowDialog()
        {
            switch (Path.GetExtension(Filename))
            {
                case ".sbc":
                case ".xml":
                    Parser.ParseBlueprint(Filename);
                    break;
                case ".sbb":
                case ".zip":
                    Parser.ParseBlueprintPackage(Filename);
                    break;
                default:
                    Close(ImportResult.Invalid);
                    break;
            }
        }

        private void Close(ImportResult result)
        {
            OnComplete?.Invoke(this, result);

            Dispatcher.Invoke(Close);
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Parser.Abort();
            this.Close(ImportResult.Aborted);
        }
    }
}
