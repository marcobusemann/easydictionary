using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace EasyDictionary.Domain
{
    /// Represents a single translation.
    /// Eg.: House (en_US) -> Haus (de_DE)
    public class Translation
    {
        public String Source { get; set; }
        public String Destination { get; set; }

        public RelayCommand CopySourceToClipboardCommand => new RelayCommand(() => CopyToClipboard(Source), () => true);
        public RelayCommand CopyDestinationToClipboardCommand => new RelayCommand(() => CopyToClipboard(Destination), () => true);

        private void CopyToClipboard(String value)
        {
            DataPackage p = new DataPackage();
            p.SetText(value);
            Clipboard.SetContent(p);
            Clipboard.Flush();
        }
    }
}
