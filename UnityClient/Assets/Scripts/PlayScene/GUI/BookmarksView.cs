using UnityEngine;

public class BookmarksView : MonoBehaviour {
    [SerializeField] BookmarkLineView linePrefab = null;

    public Bookmark Bookmark { get; private set; }
}
