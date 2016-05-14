using ChaoWorld2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Entities
{
  public interface IContainer
  {
    Item[] GetInventory();
  }
}
