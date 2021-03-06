using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils
{
  /*
  Basic unit that the renderer stores and loops over for drawing
  */

  public partial class Node : IDisposable
  {
    //internal bool Loaded = false;
    private bool Disposed = false;

    /*
    Parent/ child
    */
    public Node Parent { get; private set; }
    public List<Node> Nodes
    {
      get
      {
        return _nodes;
      }
      private set
      {
        _nodes = value;
      }
    }
    private List<Node> _nodes = new List<Node>();

    public Collider Collider;

    /*
    Rendering
    */
    public Effect Effect;
    protected Vector2 DrawPosition { get; private set; }
    protected float DrawAlpha { get; private set; }
    public Vector2 Position = new Vector2(0, 0);
    public Vector2 Origin = new Vector2(0, 0);
    public Size Size = new Size(0, 0);
    public float Rotation = 0f;
    public float Scale = 1f;
    public float Direction = 0f;
    public Color Color = Color.White;
    public float Alpha = 1;
    public float Depth = 0f;
    public bool ShowCenter = false;
    public bool Visible = true;
    public bool Active = true;

    public Node()
    {
      DrawPosition = Vector2.Zero;
      DrawAlpha = 1f;
    }

    public void Draw()
    {
      if (!Visible) return;

      if (Parent != null)
      {
        DrawPosition = Position + Parent.DrawPosition;
        DrawAlpha = Alpha * Parent.DrawAlpha;
      }
      else
      {
        DrawPosition = Position;
        DrawAlpha = Alpha;
      }

      if (Engine.CurrentRenderer.CurrentEffect != Effect)
      {
        Engine.CurrentRenderer.ApplyEffect(Effect);
      }

      Render();

      if (ShowCenter)
      {
        RenderCenter();
      }

      for (int i = 0; i < _nodes.Count; i++)
      {
        Node n = _nodes[i];
        n.Draw();
      }
    }

    protected virtual void Render()
    {
      Engine.SpriteBatch.Draw(
        Engine.SystemRect,
        new Rectangle(
          (int)(DrawPosition.X),
          (int)(DrawPosition.Y),
          (int)Size.Width,
          (int)Size.Height
        ),
        null,
        Color * DrawAlpha,
        Rotation,
        //new Vector2(Size.Width, Size.Height) / Origin,
        //Origin,
        Origin / new Vector2(Size.Width, Size.Height),
        SpriteEffects.None,
        Depth
      );
    }

    private void RenderCenter()
    {
      Engine.SpriteBatch.Draw(
        Engine.SystemRect,
        new Rectangle(
          (int)(DrawPosition.X),
          (int)(DrawPosition.Y),
          1,
          1
        ),
        null,
        Color.Red,
        0.0f,
        new Vector2(0, 0),
        SpriteEffects.None,
        0.0f
      );
    }

    public virtual void Update()
    {
      // if (!Loaded)
      // {
      //   Loaded = true;
      //   return;
      // }

      // if (Parent != null)
      // {
      //   DrawPosition = Position + Parent.DrawPosition;
      // }
      // else
      // {
      //   DrawPosition = Position;
      // }

      for (int i = 0; i < _nodes.Count; i++)
      {
        Node n = _nodes[i];
        if (n.Active)
        {
          n.Update();
        }
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
      //Console.WriteLine("\tdisposing node");
    }

    protected virtual void Dispose(bool disposing) { }

    ~Node()
    {
      Dispose(false);
    }

    public virtual void AddChild(Node n)
    {
      if (n.Parent == null)
      {
        _nodes.Add(n);
        n.Parent = this;
        n.DrawPosition = n.Position + DrawPosition;
        n.DrawAlpha = n.Alpha * DrawAlpha;
        n.Init();
        //n.SetRenderer(this.Renderer);
      }
    }

    protected virtual void Init() { }

    public void RemoveFromParent()
    {
      if (Parent != null)
      {
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
    private void RemoveAll()
    {
      foreach (Node n in _nodes)
      {
        n.RemoveAll();
      }

      _nodes.Clear();
      Parent = null;
      //Renderer = null;
      Dispose();
    }

    // public void SwapChildren(Node other)
    // {
    //   List<Node> tmp = other.Nodes;
    //   other.Nodes = Nodes;
    //   Nodes = tmp;

    //   foreach (Node n in other.Nodes)
    //   {
    //     n.Parent = n;
    //   }
    //   foreach (Node n in Nodes)
    //   {
    //     n.Parent = this;
    //   }
    // }

    public float Left
    {
      get
      {
        return Position.X - Origin.X;
      }
    }

    public float Right
    {
      get
      {
        return (Position.X + Size.Width) - Origin.X;
      }
    }

    public float Top
    {
      get
      {
        return Position.Y - Origin.Y;
      }
    }

    public float Bottom
    {
      get
      {
        return (Position.Y + Size.Height) - Origin.Y;
      }
    }
  }
}