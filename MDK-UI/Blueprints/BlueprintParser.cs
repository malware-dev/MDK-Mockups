using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using IngameScript.Mockups;
using IngameScript.Mockups.Base;
using IngameScript.Mockups.Blocks;
using VRage.Game;

namespace MDK_UI.Blueprints
{
    public class BlueprintParser
    {
        private Task _currentTask;
        private CancellationTokenSource _cancellationToken;

        public class OnProgressArgs
        {
            public long Current { get; }
            public long Total { get; }

            public OnProgressArgs(long current, long total)
            {
                Current = current;
                Total = total;
            }
        }

        /// <summary>
        /// Blueprint will be empty if import failed.
        /// </summary>
        public event EventHandler<Blueprints.BlueprintDefinition> Completed;
        public event EventHandler<OnProgressArgs> Progress;

        public bool ParseBlueprint(string filename)
        {
            return StartIf(() =>
            {
                try
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        ParseBlueprint(stream);
                    }
                }
                catch (FileNotFoundException)
                {
                    OnCompleted();
                }
            });
        }

        public bool ParseBlueprintPackage(string filename)
        {
            return StartIf(() =>
            {
                try
                {
                    using (var archive = File.OpenRead(filename))
                    using (var zip = new ZipArchive(archive, ZipArchiveMode.Read))
                    {
                        var file = zip.Entries.SingleOrDefault(e => Path.GetExtension(e.Name) == ".sbc");

                        // Not a supported Blueprint archive.
                        if (file == default(ZipArchiveEntry))
                            OnCompleted();

                        using (var stream = file.Open())
                        {
                            ParseBlueprint(stream);
                        }
                    }
                }
                // Not a real filename.
                catch (FileNotFoundException)
                {
                    OnCompleted();
                }
                // Not a valid ZIP file.
                catch (InvalidDataException)
                {
                    OnCompleted();
                }
            });
        }

        public void Abort()
        {
            _cancellationToken.Cancel();
            _currentTask?.Wait();
        }

        private bool StartIf(Action action)
        {
            if (_currentTask?.IsCompleted ?? true == false)
                return false;

            _cancellationToken = new CancellationTokenSource();
            _currentTask = Task.Factory.StartNew(action);
            return true;
        }

        private void ParseBlueprint(Stream stream)
        {
            //OnProgress(stream.Position, stream.Length);

            var name = "";
            var size = MyCubeSize.Large;
            var isStatic = true;
            var unsupported = false;

            var blocks = new List<MockTerminalBlock>();
            var groups = new List<MockBlockGroup>();

            
            var reader = new XmlSerializer(typeof(MyObjectBuilder_Definitions));
            try
            {
                var definition = (MyObjectBuilder_Definitions)reader.Deserialize(stream);
            }
            catch
            {
                OnCompleted();
                return;
            }

            if (_cancellationToken.IsCancellationRequested)
            {
                OnCompleted();
            }
            else
            {
                if (name == "")
                {
                    OnCompleted();
                }
                else
                {
                    OnCompleted(new BlueprintDefinition(name, size, isStatic, unsupported, groups, blocks));
                }
            }
        }

        private void OnProgress(long current, long total)
            => Progress?.Invoke(this, new OnProgressArgs(current, total));

        private void OnCompleted(BlueprintDefinition blueprint = null)
            => Completed?.Invoke(this, blueprint);
    }
}
