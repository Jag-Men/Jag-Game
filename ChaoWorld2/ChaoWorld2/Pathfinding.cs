using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  public class PathFinding
  {
    public Entity Entity;
    public Vector2 Goal;

    public int PathPosition = 0;
    public PathTile[] FinalPath;

    public PathFinding(Entity owner)
    {
      this.Entity = owner;
    }

    public void Pathfind(Vector2 goal, params string[] collisions)
    {
      this.Goal = goal;
      List<PathTile> OpenList = new List<PathTile>();
      List<PathTile> ClosedList = new List<PathTile>();
      OpenList.Add(new PathTile(Utility.GetTilePos(Entity.X, Entity.Y), null));
      PathTile currentTile = null;
      while(OpenList.Count > 0)
      {
        currentTile = null;
        foreach (var tile in OpenList)
        {
          if (currentTile == null || tile.FScore(Goal) < currentTile.FScore(Goal))
            currentTile = tile;
        }

        List<PathTile> RemovedTiles = new List<PathTile>();
        foreach (var tile in OpenList)
          if (tile.Pos.Equals(currentTile.Pos))
            RemovedTiles.Add(tile);
        foreach (var tile in RemovedTiles)
          OpenList.Remove(tile);
        ClosedList.Add(currentTile);

        if (currentTile.Pos.Equals(Goal))
          break;

        Console.WriteLine(currentTile.Pos.X + ", " + currentTile.Pos.Y);

        Vector2 currentPos = currentTile.Pos;
        List<PathTile> adjacentTiles = new List<PathTile>();
        if (IsTilePassable(currentPos.X, currentPos.Y + 1, currentTile, collisions))
          adjacentTiles.Add(new PathTile(currentPos.X, currentPos.Y + 1, currentTile));
        if (IsTilePassable(currentPos.X - 1, currentPos.Y, currentTile, collisions))
          adjacentTiles.Add(new PathTile(currentPos.X - 1, currentPos.Y, currentTile));
        if (IsTilePassable(currentPos.X, currentPos.Y - 1, currentTile, collisions))
          adjacentTiles.Add(new PathTile(currentPos.X, currentPos.Y - 1, currentTile));
        if (IsTilePassable(currentPos.X + 1, currentPos.Y, currentTile, collisions))
          adjacentTiles.Add(new PathTile(currentPos.X + 1, currentPos.Y, currentTile));

        foreach (var adjTile in adjacentTiles)
        {
          bool addToOpen = true;
          foreach (var closedTile in ClosedList)
            if (closedTile.Pos.Equals(adjTile.Pos))
              addToOpen = false;
          if (!addToOpen)
            continue;
          foreach (var openTile in OpenList)
            if (openTile.Pos.Equals(adjTile.Pos))
              if (adjTile.FScore(Goal) < openTile.FScore(Goal))
              {
                openTile.Parent = adjTile.Parent;
                addToOpen = false;
              }
          if (addToOpen)
            OpenList.Add(adjTile);
        }
      }
      if (currentTile.Pos.Equals(Goal))
      {
        List<PathTile> Path = new List<PathTile>();
        PathTile newTile = currentTile;
        while (newTile != null)
        {
          Path.Add(newTile);
          newTile = newTile.Parent;
        }
        Path.Reverse();
        FinalPath = Path.ToArray();
      }
    }

    bool IsTilePassable(float x, float y, PathTile parent, params string[] collisions)
    {
      Vector2 entityTilePos = Utility.GetTilePos(Entity.X, Entity.Y);
      Rectangle parentRect = new Rectangle(Entity.GetCollisionBox().X, Entity.GetCollisionBox().Y, Entity.GetCollisionBox().Width, Entity.GetCollisionBox().Height);
      parentRect.Offset(-(int)(entityTilePos.X * Game1.TileSize), -(int)(entityTilePos.Y * Game1.TileSize));
      parentRect.Offset((int)parent.Pos.X * Game1.TileSize, (int)parent.Pos.Y * Game1.TileSize);
      Rectangle targetRect = new Rectangle(Entity.GetCollisionBox().X, Entity.GetCollisionBox().Y, Entity.GetCollisionBox().Width, Entity.GetCollisionBox().Height);
      targetRect.Offset(-(int)(entityTilePos.X * Game1.TileSize), -(int)(entityTilePos.Y * Game1.TileSize));
      targetRect.Offset((int)x * Game1.TileSize, (int)y * Game1.TileSize);
      Rectangle checkRect = Rectangle.Union(parentRect, targetRect);
      if (Entity.Owner.RectCollidesWith(checkRect, collisions))
        return false;
      return true;
    }
  }

  public class PathTile
  {
    public Vector2 Pos;
    public PathTile Parent;

    public PathTile(Vector2 pos, PathTile parent)
    {
      this.Pos = pos;
      this.Parent = parent;
    }

    public PathTile(float x, float y, PathTile parent)
      :this(new Vector2(x, y), parent)
    {

    }

    public bool ContainsParent(PathTile parent)
    {
      if (this.Parent == null)
        return false;
      if (this.Parent == parent)
        return true;
      return this.Parent.ContainsParent(parent);
    }

    public int GScore()
    {
      if (this.Parent == null)
        return 0;
      return this.Parent.GScore() + 1;
    }

    public int FScore(Vector2 goal)
    {
      return this.GScore() + Utility.ManhattanDist(this.Pos, goal);
    }
  }
}
