using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {
  /*
  Basic unit that the renderer stores and loops over for drawing
  */

  public partial class Node : IDisposable {

    private bool Disposed = false;

    // child node stuff
    private List<Node> _nodes = new List<Node>();
    public List<Node> Nodes {
      get {
        return _nodes;
      }
      private set {
        _nodes = value;
      }
    }

    public Node Parent { get; private set; }


    // Position stuff
    public float X;
    public float Y;

    // Drawing stuff
    public bool IsHidden = false;
    public bool ShowCenter = false;

    public Node() { }

    public void AddChild(Node n) {
      if (n.Parent == null) {
        _nodes.Add(n);
        n.Parent = this;
      }
    }

    public void RemoveFromParent() {
      if (Parent != null) {
        Parent._nodes.Remove(this);

        RemoveAll();
      }
    }

    /*
    RemoveAll() is separate from RemoveFromParent(), because we cannot manipulate a collection,
    such as _nodes, while we are iterating over it. The only Node that needs to sever itself
    from its parent is the calling Node, so we manually remove it with RemoveFromParent(), 
    then use RemoveAll() to do the actual loop to clear and dispose the calling Node and 
    its kiddos.
    */

    private void RemoveAll() {
      foreach (Node n in _nodes) {
        n.RemoveAll();
      }

      _nodes.Clear();
      Parent = null;
      Dispose();
    }

    public virtual void Draw(SpriteBatch spriteBatch, float lastX = 0, float lastY = 0) {
      if (IsHidden) return;

      float worldX = lastX + X;
      float worldY = lastY + Y;

      Bounds.Draw(spriteBatch, worldX, worldY);

      // for position debugging
      if (ShowCenter) {
        spriteBatch.Draw(
          BoundingBox.Texture,
          new Rectangle((int)(worldX), (int)(worldY), 2, 2),
          Color.Red
        );
      }

      for (int i = 0; i < _nodes.Count; i++) {
        Node n = _nodes[i];
        n.Draw(spriteBatch, worldX, worldY);
      }

    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) { }

    ~Node() {
      Dispose(false);
    }
  }
}