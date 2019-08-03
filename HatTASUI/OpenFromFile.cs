using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace HatTASUI
{
    public static class OpenFromFile
    {
        public static TASFile? Open()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Hat TAS File (*.htas)|*.htas|All Files (*.*)|*.*";
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return Open(dialog.FileName);
                }
            }
            return null;
        }

        private static TASFile? Open(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                var metadata = new Metadata();
                var frames = new List<Frame>();
                foreach (var line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (char.IsDigit(line[0]))
                        {
							var frame = Frame.FromString(line);
                            if(frame != null) frames.Add(frame);
                        }
                        else
                        {
                            metadata.UpdateFromString(line);
                        }
                    }
                }
                return new TASFile(frames, metadata, filePath);
            }
            catch
            {
                MessageBox.Show("The file could not be opened.", "File Opening Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }
    }
}
