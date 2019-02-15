using System;

namespace EasyApp.Documentation
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class HelpAttribute : Attribute
    {
        private ArgumentInfo _option;

        public string Description
        {
            get;
            set;
        }
        public string Example
        {
            get;
            set;
        }
        public int Order
        {
            get;
            set;
        }
        public string Group
        {
            get;
            set;
        }

        public HelpAttribute(string description)
        {
            this.Description = description;
            this.Order = 0;
        }

        public void SetOption(ArgumentInfo option)
        {
            _option = option;
        }

        public ArgumentInfo GetOption()
        {
            return _option;
        }
    }
}
