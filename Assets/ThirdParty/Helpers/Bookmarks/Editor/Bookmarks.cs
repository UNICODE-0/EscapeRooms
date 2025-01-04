using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSinger.BookmarksAndSelections
{
    public class Bookmarks : ScriptableObject
    {
        [SerializeField]
        private List<Bookmark> bookmarks = new();
            
        public List<Bookmark> List
        {
            get => bookmarks;
            set => bookmarks = value;
        }
    }

    [Serializable]
    public class Bookmark
    {
        public UnityEngine.Object bookmarkObject;
        public string descriprion;

        public Bookmark(UnityEngine.Object bookmarkObject, string descriprion)
        {
            this.bookmarkObject = bookmarkObject;
            this.descriprion = descriprion;
        }
    }
}