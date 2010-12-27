/**************************************************************************
    Subsonic Csharp
    Copyright (C) 2010  Ian Fijolek
 
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
**************************************************************************/

using System;
using Gtk;
using SubsonicAPI;
using System.Collections.Generic;
using HollyLibrary;

public partial class MainWindow : Gtk.Window
{
	HTreeView theTreeView;
	
	public MainWindow () : base(Gtk.WindowType.Toplevel)
	{
		Build ();
		Subsonic.appName = "IansCsharpApp";
		
		InitializeTreeView();
	}
	
	private void InitializeTreeView()
	{
		theTreeView = new HTreeView();
		swLibrary.Add(theTreeView);
		theTreeView.NodeExpanded += HandleTheTreeViewNodeExpanded;
		theTreeView.Editable = false;
		theTreeView.Visible = true;
	}

	void HandleTheTreeViewNodeExpanded (object sender, NodeEventArgs args)
	{
		// Fetch any items inside the node...
		HTreeNode thisNode = args.Node;
		
		// Check to see if it has any children
		if (thisNode.Nodes.Count == 1 && thisNode.Nodes[0].Text == "")
		{
			// Node child is a dummy
			thisNode.Nodes[0].Text = "Loading...";
			
			// Get path to the selected node to expandsimp
			Queue<string> nodePath = GetNodePath(thisNode);
			
			// Dive into library to selected node
			SubsonicItem thisItem = Subsonic.MyLibrary;
			while (nodePath.Count > 0)
			{
				thisItem = thisItem.GetChildByName(nodePath.Dequeue());
			}
			
			// Should now have the correct selected item
			foreach(SubsonicItem child in thisItem.children)
			{
				HTreeNode childNode = new HTreeNode(child.name);
				thisNode.Nodes.Add(childNode);
				
				// Adding a dummy node for any Folders
				if (child.itemType == SubsonicItem.SubsonicItemType.Folder)
					childNode.Nodes.Add(new HTreeNode(""));
			}			
			
			// Remove dummy node
			thisNode.Nodes.RemoveAt(0);
		}		
	}

	void HandleTheTreeViewBeforeNodeExpand (object sender, NodeEventArgs args)
	{
		// Fetch any items inside the node...
		HTreeNode thisNode = args.Node;
		
		// Check to see if it has any children
		if (thisNode.Nodes.Count == 1 && thisNode.Nodes[0].Text == "")
		{
			// Node child is a dummy
			//thisNode.Icon = new Gdk.Pixbuf("ItemLoad.gif");
			
			// Get path to the selected node to expandsimp
			Queue<string> nodePath = GetNodePath(thisNode);
			
			// Dive into library to selected node
			SubsonicItem thisItem = Subsonic.MyLibrary;
			while (nodePath.Count > 0)
			{
				thisItem = thisItem.GetChildByName(nodePath.Dequeue());
			}
			
			// Should now have the correct selected item
			foreach(SubsonicItem child in thisItem.children)
			{
				HTreeNode childNode = new HTreeNode(child.name);
				thisNode.Nodes.Add(childNode);
				
				// Adding a dummy node for any Folders
				if (child.itemType == SubsonicItem.SubsonicItemType.Folder)
					childNode.Nodes.Add(new HTreeNode(""));
			}			
			
			// Remove dummy node
			thisNode.Nodes.RemoveAt(0);
			//thisNode.Icon = null;
		}
	}
	
	private Queue<string> GetNodePath(HTreeNode theNode)
	{
		Queue<string> nodePath;
		if (theNode.ParentNode != null)
		{
			nodePath = GetNodePath(theNode.ParentNode);
		}
		else
		{
			nodePath = new Queue<string>();
		}
		
		nodePath.Enqueue(theNode.Text);
		
		return nodePath;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
	
	protected virtual void OnBtnLoginClicked (object sender, System.EventArgs e)
	{
		
	}
	
	protected virtual void OnBtnGetAlbumsClicked (object sender, System.EventArgs e)
	{
		
	}
	
	protected virtual void OnBtnLogin2Clicked (object sender, System.EventArgs e)
	{
		string server = tbServer.Text;
		string user = tbUsername.Text;
		string passw0rdd = tbPaassw0rd.Text;
		
		
		
		string loginResult = Subsonic.LogIn(server, user, passw0rdd);
		Console.WriteLine("Login Result: " + loginResult);
				
		SubsonicItem thisLibrary = Subsonic.MyLibrary;
		foreach(SubsonicItem artist in thisLibrary.children)
		{
			HTreeNode artistNode = new HTreeNode(artist.name);
			theTreeView.Nodes.Add(artistNode);
			
			// Adding a dummy node for the artist
			artistNode.Nodes.Add(new HTreeNode(""));
		}
	}
	
	
	
	
}

