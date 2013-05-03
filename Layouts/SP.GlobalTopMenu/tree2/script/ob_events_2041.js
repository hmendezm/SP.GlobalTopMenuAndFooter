function ob_OnNodeDrop(src, dst, copy)
{    
    // add client side code here	
    //alert("Node with id:" + src + " was " + (!copy ? "moved" : "copied") + " to node with id:" + dst);
    	   
	if(ob_ev("OnNodeDrop"))
	{
		if(document.getElementById(dst).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			dst = "root";
		} 
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("src", src);
	        ob_post.AddParam("dst", dst);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeDrop");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}
}

function ob_OnNodeDropOutside(dst)
{    
    // add client side code here
    //alert("ob_OnNodeDropOutside");
    
    ob_t2_CopyToControl(dst); // comment this line if you don't want to drop nodes into textboxes
    
    if(ob_ev("OnNodeDropOutside"))
	{
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("dst", dst);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeDropOutside");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}
}  


function ob_OnNodeSelect(id)
{       
     // add client side code here
	 //alert("OnNodeSelect on " + id);	 	 	 
	 
	 if(ob_ev("OnNodeSelect"))
	 {	    
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		} 
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeSelect");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	 }
}

function ob_OnNodeEdit(id, text, prevText)
{    
    // add client side code here
	//alert("OnNodeEdit on " + id + "\n  text: " + text + "\n prevText: " + prevText);
		
	if(ob_ev("OnNodeEdit"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);
	        ob_post.AddParam("text", text);
	        ob_post.AddParam("prevText", prevText);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeEdit");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	} 
}

function ob_OnAddNode(parentId, childId, textOrHTML, expanded, image, subTreeURL)
{    
	// add client side code here
	/*alert("OnAddNode:\n  parentId: " + (parentId || "none")
			+ "\n  childId: " + (childId || "none")
			+ "\n  textOrHTML: " + (textOrHTML || "none")
			+ "\n  expanded: " + (expanded || "false")
			+ "\n  image: " + (image || "none")
			+ "\n  subTreeURL: " + (subTreeURL || "none"));*/
    
	if(ob_ev("OnAddNode"))
	{
		if(document.getElementById(parentId).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			parentId = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("parentId", parentId);
	        ob_post.AddParam("childId", childId);
	        ob_post.AddParam("textOrHTML", textOrHTML);
	        ob_post.AddParam("expanded", expanded ? expanded : 0);
	        ob_post.AddParam("image", image ? image : "");
	        ob_post.AddParam("subTreeURL", subTreeURL ? subTreeURL : "");
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnAddNode");
	    } 		
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
}

function ob_OnRemoveNode(id)
{    
     // add client side code here
	 //alert("OnRemoveNode on " + id);
	 	 
	 if(ob_ev("OnRemoveNode"))
	 {		
	    if(typeof ob_post == "object")
	    {			
	        ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnRemoveNode");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	 }
}

function ob_OnNodeExpand(id, dynamic)
{
    // add client side code here	
	//alert("OnNodeExpand on: " + id + " " + dynamic);
        
    if(ob_ev("OnNodeExpand"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);		        
	        ob_post.AddParam("parentId", id);        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeExpand");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

function ob_OnNodeCollapse(id)
{
    // add client side code here
	// alert("OnNodeCollapse on " + id);			
	
    if(ob_ev("OnNodeCollapse"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeCollapse");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

function ob_OnMoveNodeUp(node_up_id, node_down_id)
{
    // add client side code here
	// alert("OnMoveNodeUp: Node with id '" + node_up_id + "' was moved up. It is located before the node with id '" + node_down_id + "'.");
	
    if(ob_ev("OnMoveNodeUp"))
	{		
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("node_up_id", node_up_id);
	        ob_post.AddParam("node_down_id", node_down_id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnNodeMoveUp");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

function ob_OnMoveNodeDown(node_down_id, node_up_id)
{
    // add client side code here
	// alert("OnMoveNodeUp: Node with id '" + node_down_id + "' was moved down. It is located after the node with id '" + node_up_id + "'.");
	
    if(ob_ev("OnMoveNodeDown"))
	{		
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("node_down_id", node_down_id);
	        ob_post.AddParam("node_up_id", node_up_id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnMoveNodeDown");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}		
}

/*
	Pre-events.
	Use them to implement your own validation for such operations as add, remove, edit
*/

function ob_OnBeforeAddNode(parentId, childId, textOrHTML, expanded, image, subTreeURL)
{        
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeAddNode");			
	if(ob_ev("OnBeforeAddNode"))
	{
		if(document.getElementById(parentId).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			parentId = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("parentId", parentId);
	        ob_post.AddParam("childId", childId);
	        ob_post.AddParam("textOrHTML", textOrHTML);
	        ob_post.AddParam("expanded", expanded ? expanded : 0);
	        ob_post.AddParam("image", image ? image : "");
	        ob_post.AddParam("subTreeURL", subTreeURL ? subTreeURL : "");
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeAddNode");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	} 	
	return true;
}

function ob_OnBeforeRemoveNode(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeRemoveNode");	
	if(ob_ev("OnBeforeRemoveNode"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeRemoveNode");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
	return true;
}

function ob_OnBeforeNodeEdit(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeNodeEdit");
	if(ob_ev("OnBeforeNodeEdit"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeEdit");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
	return true;
}

function ob_OnBeforeNodeSelect(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeNodeSelect");
	
	if(ob_ev("OnBeforeNodeSelect"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeSelect");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    } 
	}
	return true;
}

function ob_OnBeforeNodeDrop(src, dst, copy)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid	
	//alert("Node with id:" + src + " will be " + (!copy ? "moved" : "copied") + " to node with id:" + dst);	
	if(ob_ev("OnBeforeNodeDrop"))
	{
		if(document.getElementById(dst).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			dst = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("src", src);
	        ob_post.AddParam("dst", dst);
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeDrop");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	return true;
}

function ob_OnBeforeNodeDrag(id)
{    
	// add your own validation code
	// e.g. it may use synchronized obout postback to query
	// server side application whether such operation is valid
	//alert("OnBeforeNodeDrag for node with id: " + id);
	
	if(ob_ev("OnBeforeNodeDrag"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeDrag");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	return true;
}


function ob_OnBeforeNodeDropOutside(dst)
{    
    // add client side code here
    //alert("ob_OnBeforeNodeDropOutside");    
    if(ob_ev("OnBeforeNodeDropOutside"))
	{
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("dst", dst);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeDropOutside");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}
	
	return true;
} 

function ob_OnBeforeNodeExpand(id, dynamic)
{
    // add client side code here
	//alert("OnBeforeNodeExpand on: " + id + " " + dynamic);
    if(ob_ev("OnBeforeNodeExpand"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeExpand");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	
	return true;	
}

function ob_OnBeforeNodeCollapse(id)
{
    // add client side code here
	//alert("OnBeforeNodeCollapse on " + id);			
		
    if(ob_ev("OnBeforeNodeCollapse"))
	{
		if(document.getElementById(id).parentNode.parentNode.firstChild.firstChild.className == "ob_t8") {
			id = "root";
		}
	    if(typeof ob_post == "object")
	    {
	        ob_post.AddParam("id", id);	        
	        //Change "TreeEvents.aspx" with the name of your server-side processing file
	        ob_post.post("TreeEvents.aspx", "OnBeforeNodeCollapse");
	    }
	    else
	    {
	        alert("Please add obout_AJAXPage control to your page to use the server-side events");
	    }
	}	
	
	return true;
}
