using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rejestracja.Utils {
    class PrintOptions {
        private List<KeyValuePair<string, int>> _entries;
        private bool _printAsOneDocument;

        public List<KeyValuePair<string, int>> entries {
            get { return _entries; }
            set { _entries = value; }
        }

        public bool printAsOneDocument {
            get { return _printAsOneDocument; }
            set { _printAsOneDocument = value; }
        }


        public PrintOptions() { }
        public PrintOptions(List<KeyValuePair<string, int>> entries, bool printAsOneDocument) {
            this._entries = entries;
            this._printAsOneDocument = printAsOneDocument;
        }
    }
}
