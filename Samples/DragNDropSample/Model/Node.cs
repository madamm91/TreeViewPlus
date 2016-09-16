namespace W7StyleSample.Model
{
   #region

    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Controls.DragNDrop;
    using System.Linq;
   #endregion

   /// <summary>
   /// Model for testing
   /// </summary>
   public class Node
   {
      #region Constructors and Destructors

      public Node()
      {
         Children = new ObservableCollection<Node>();
      }

      public bool AllowDrag { get; set; }

      public bool CanDrag()
      {
         return AllowDrag;
      }

      public object OnDrag()
      {
          return this;
      }

      #endregion

      #region Public Properties

      public ObservableCollection<Node> Children { get; set; }

      public string Name { get; set; }
      #endregion

      #region Public Methods

      /*public override string ToString()
      {
         return Name;
      }*/

      #endregion
   }
}