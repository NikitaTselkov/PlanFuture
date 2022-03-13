using System;
using System.Windows.Controls;
using System.Windows.Markup;

namespace PlanFuture.Modules.NavigationModule.Converters
{
    public partial class CaseConverter : MarkupExtension
    {
        private CharacterCasing sourceCasing;
        private CharacterCasing targetCasing;

        public CaseConverter()
        {
        }

        public CaseConverter(CharacterCasing casing)
        {
            this.Casing = casing;
        }

        public CaseConverter(CharacterCasing sourceCasing, CharacterCasing targetCasing)
        {
            this.SourceCasing = sourceCasing;
            this.TargetCasing = targetCasing;
        }

        public CharacterCasing SourceCasing
        {
            get
            {
                return this.sourceCasing;
            }

            set
            {
                this.sourceCasing = value;
            }
        }

        public CharacterCasing TargetCasing
        {
            get
            {
                return this.targetCasing;
            }

            set
            {
                this.targetCasing = value;
            }
        }

        public CharacterCasing Casing
        {
            set
            {
                this.sourceCasing = value;
                this.targetCasing = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new CaseConverter(this.SourceCasing, this.TargetCasing);
        }
    }
}
