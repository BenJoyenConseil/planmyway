using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanMyWay.Html
{
    public class Element
    {
        List<Element> _children = new List<Element>();
        String _tag = string.Empty;
        String _text = string.Empty;

        /// <summary>
        /// Text sans formatage à afficher. S'affiche avant les balises enfants s'ils en existe.
        /// </summary>
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// Set : Entrer le nom de la balise sans les chevrons
        /// exemple : table et non <table>
        /// </summary>
        public String Tag
        {
            get { return "<" + _tag + ">"; }
            set { _tag = value; }
        }

        public String TagClose
        {
            get { return "</" + _tag + ">"; }
        }

        public List<Element> Children
        {
            get { return _children; }
        }

        public void Add(Element element)
        {
            _children.Add(element);
        }

        public Element(String tag = "html")
        {
            Tag = tag;
        }

        public override string ToString()
        {
            string str = string.Empty;
            str += Tag;
            str += _text;
            foreach (var child in _children)
                str += child.ToString();
            str += TagClose;
            return str;
        }
    }
}
