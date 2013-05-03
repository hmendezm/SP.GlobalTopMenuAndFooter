//<!--
//	  ASP TreeView
//  
//	  Copyright obout inc	  http://www.obout.com

var ob_tree_js_version="2041",ob_page_location=window.location.pathname.toString().substr(window.location.pathname.toString().lastIndexOf("/") + 1),currentlySelected,ob_KeepLastExpanded=0,ob_KeepLastCollapsed=0,ob_tree_first_call=true,tree_node_exp_col=false,ob_last,ob_sids=null,ob_ivl="",ob_lastControlSelected="", ob_iar=0,ob_op2,tree_selected_id,ob_prev_selected,tree_parent_id,tree_selected_path,ob_xmlhttp2,ob_alert2,ob_eids,ob_cids, ob_sidsDone,ob_csidsDone,ob_t2_OnDynamicLoad,ob_t2_OnDynamicLoad2,ob_t2_Data;if(typeof ob_pbxs=="undefined")ob_pbxs=[];ob_tree_callback=null;if(typeof ob_tree_dl=="undefined")ob_tree_dl=[];function ob_tree_InitPbx(){var ob_pbx1=null;if (window.XMLHttpRequest){try{ob_pbx1=new window.XMLHttpRequest();}catch(e){}}else{try{ob_pbx1=new ActiveXObject("Microsoft.XMLHTTP");}catch(e){try{ob_pbx1=new ActiveXObject('msxml2.xmlhttp')}catch(e){}}}if(ob_pbx1!=null){ob_pbxs[ob_pbxs.length]=ob_pbx1;}return ob_pbx1;};window.onbeforeunload=function(){for(var i=0;i<ob_pbxs.length;i++)try{ob_pbxs[i].abort();}catch(ex){}};function ob_rsc(ob_xmlhttp2, ob_url2, ob_tb2, callback){if(ob_xmlhttp2.readyState==4){ob_tb2.className="none";if(ob_xmlhttp2.responseText){if(ob_tb2.parentNode == null || ob_tb2.parentNode.parentNode == null || ob_tb2.parentNode.parentNode.parentNode == null || ob_tb2.parentNode.parentNode.parentNode.parentNode == null) {return;}var tempDiv = document.createElement("DIV");tempDiv.innerHTML = ob_xmlhttp2.responseText;ob_tb2.innerHTML=tempDiv.firstChild.innerHTML;ob_tb2.parentNode.parentNode.parentNode.parentNode.className="ob_d2b";if (typeof ob_t18 != 'undefined')ob_t18(ob_tb2);if(tempDiv.childNodes[1].firstChild) {eval(ob_tHtmlDecode(tempDiv.childNodes[1].innerHTML.toString()));}if(tempDiv.childNodes[2].firstChild) {if(typeof(ob_tnodrag) == "function") {ob_tnodrag(tempDiv.childNodes[2].innerHTML.toString());}}if(tempDiv.childNodes[3].firstChild) {if(typeof(sNoDrop) != "undefined") {if(sNoDrop != "") { sNoDrop += ",";};sNoDrop += tempDiv.childNodes[3].innerHTML.toString();}}if(tempDiv.childNodes[4].firstChild) {if(typeof(ob_noedit) != "undefined") {if(ob_noedit != "") { ob_noedit += ",";};ob_noedit += tempDiv.childNodes[4].innerHTML.toString();}}if(tempDiv.childNodes[5].firstChild) {if(typeof(sNoSelect) != "undefined") {if(sNoSelect != "") { sNoSelect += ",";};sNoSelect += tempDiv.childNodes[5].innerHTML.toString();}}} var ob_tempId=ob_tb2.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.childNodes[2].id;if(ob_tb2.innerHTML == "Loading ..."){var ob_tempImg=ob_tb2.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.firstChild;ob_tempImg.src=ob_tempImg.src.toString().replace("minus_l.gif", "hr_l.gif").replace("minus.gif", "hr_l.gif");ob_tb2.parentNode.parentNode.parentNode.parentNode.className="";oTempLoadingNode=ob_tb2.parentNode.parentNode.parentNode;oTempLoadingNode.parentNode.removeChild(oTempLoadingNode);}if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true){ob_t51();} ob_OnNodeExpand(ob_tempId, ob_url2 != null && ob_url2.length > 0);if(typeof(ob_t2_OnDynamicLoad) == "function"){ob_t2_OnDynamicLoad();ob_t2_OnDynamicLoad=null;}if(typeof(ob_t2_OnDynamicLoad2) == "function"){ob_t2_OnDynamicLoad2();  ob_t2_OnDynamicLoad2=null;}if (typeof callback != 'undefined')callback();}}function ob_tHtmlDecode(sString) {if(sString == null) {return "";}sString = sString.toString();return sString.replace(/(&lt;)/g, "<").replace(/(&gt;)/g, ">").replace(/&amp;/g, "&");}function ob_t24(node, url, callback){var ob_xmlhttp2=ob_tree_InitPbx();if(ob_xmlhttp2){ob_xmlhttp2.open("GET",url,true);ob_xmlhttp2.onreadystatechange=function(){ob_rsc(ob_xmlhttp2, url, node, callback)};ob_xmlhttp2.send(null);}}function ob_t26(sids, selectLast, idx, callback){ if (typeof idx=='undefined'||idx==null){idx=ob_tree_dl.length;sids=typeof sids=="undefined"||sids==null?ob_sids:sids;try{sids=typeof sids=="object"?sids:sids.split(",");}catch(ex){}var last=typeof selectLast=="undefined"||selectLast?typeof sids=="object"&&typeof sids.length=="number"?sids.length>0?sids[sids.length-1]:ob_last:null:null;ob_tree_dl[idx]=[typeof sids=="object"&&sids!=null&&typeof sids.length=="number"?sids:[],last,0,typeof callback=="function"?callback:function(){}];}if (ob_tree_dl[idx][2] >= ob_tree_dl[idx][0].length){if (typeof ob_tree_dl[idx][1]=="string"&&ob_tree_dl[idx][1].length>0){var ob_od=document.getElementById(ob_tree_dl[idx][1]);if(ob_od){if (ob_tree_multiselect_enable){ob_unselectAllNodes();}var prevEditEnable = ob_tree_editnode_enable;ob_tree_editnode_enable = false;ob_t22(ob_od);ob_tree_editnode_enable = prevEditEnable;}else{if(!(ob_KeepLastExpanded > 0 || ob_KeepLastCollapsed > 0)){alert("Selected node does not exist.");}}}try{ob_tree_dl[idx][3]()}catch(e){};return;}else{var ob_od=document.getElementById(ob_tree_dl[idx][0][ob_tree_dl[idx][2]]);ob_tree_dl[idx][2]++;if(ob_od){if (ob_tree_multiselect_enable){ob_unselectAllNodes();var prevEditEnable = ob_tree_editnode_enable;ob_tree_editnode_enable = false;ob_t22(ob_od, null, true);ob_tree_editnode_enable = prevEditEnable;}var e,lensrc,s;e=ob_od.parentNode.firstChild.firstChild;if((typeof e.src!="undefined")&&(e.tagName=="IMG")){s=e.src.substr((e.src.length-8),8);if((s=="usik.gif")||(s=="ik_l.gif")||(s=="ik_r.gif")){eval("ob_tree_callback=function(){ob_t26(null,null,"+idx+",null);}");if (ob_KeepLastExpanded > 0 || ob_KeepLastCollapsed > 0){ if(e.onclick.toString().indexOf("this, ''")!=-1){ob_t21(e,'');} else {e.onclick();}} else e.onclick(); return;}}}else if (ob_tree_multiselect_enable && ob_od){alert("Selected node does not exist.");}}ob_t26(null,null,idx,null);}function ob_ev(e){return typeof ob_eventFlags!='undefined'&&ob_eventFlags[e];}

function ob_t21(os, url) 
{
	var callback = ob_tree_callback != null ? ob_tree_callback : function(){};
	ob_tree_callback = null;
	
	// Switch plus-minus images when clicked.	
	currentlySelected = os.parentNode.parentNode.firstChild.nextSibling.nextSibling;
	var ob_node_id = currentlySelected.id;
	var ot = os.parentNode.parentNode.parentNode.parentNode.nextSibling;    	
	
	var lensrc = (os.src.length - 8);
	var s = os.src.substr(lensrc, 8);
	
	if ((s == "inus.gif")||(s == "us_l.gif")||(s == "us_r.gif")) {
		
		if(!ob_OnBeforeNodeCollapse(ob_node_id))
			return;
		
		if (ob_KeepLastExpanded > 0 || ob_KeepLastCollapsed > 0)
		{
			ob_saveNodeState(ot, ob_node_id);
		}
		ob_changeIcon (currentlySelected);
		
		if (s == "inus.gif") 
		{
			ot.style.display = "none";
			os.src = ob_style + "/plusik.gif";
		}
		else if((s == "us_l.gif"))
		{
			ot.style.display = "none";
			os.src = ob_style + "/plusik_l.gif";
		}
		else
		{
			ot.style.display = "none";
			os.src = ob_style + "/plusik_r.gif";
		}
		
		if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true)
		{
		    ob_t51();
		}
		
		ob_OnNodeCollapse(ob_node_id);
	}

	if ((s == "usik.gif")||(s == "ik_l.gif")||(s == "ik_r.gif")) 
	{
		if(!ob_OnBeforeNodeExpand(ob_node_id, url != null && url.length > 0))
			return;
	    
	    if (ob_KeepLastExpanded > 0 || ob_KeepLastCollapsed > 0)
		{
			ob_saveNodeState(ot, ob_node_id);
		}
		ob_changeIcon (currentlySelected);
		
		if (typeof (ob_expand_single) != 'undefined' && ob_expand_single)
			ob_expandSingle(currentlySelected);
		if (typeof(url) == 'undefined' || url == "")
		{
			ot.style.display = "block";
			if(s == "usik.gif")
			{
				os.src = ob_style + "/minus.gif";
			}
			else if(s == "ik_l.gif")
			{
				os.src = ob_style + "/minus_l.gif";
			}
			else
			{
				os.src = ob_style + "/minus_r.gif";
			}
			if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true)
			{
				ob_t51();
			}
			ob_OnNodeExpand(ob_node_id, url != null && url.length > 0);
			
			callback();
		}
		else
		{
			ot.style.display = "block";
			if(s == "usik.gif")
			{
				os.src = ob_style + "/minus.gif";
			}
			else if(s == "ik_l.gif")
			{
				os.src = ob_style + "/minus_l.gif";
			}
			if (typeof(url) != 'undefined' && url != "") 
			{
				window.status = url;
				var s = os.parentNode.parentNode.parentNode.parentNode.nextSibling.firstChild.firstChild.firstChild.nextSibling.innerHTML;
				if (s != "Loading ...") 
				{
					if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true)
					{
						ob_t51();
					}
				    ob_OnNodeExpand(ob_node_id, url != null && url.length > 0);
					return;
				}
				//ob_url2 = url; 
				var ob_tb2 = os.parentNode.parentNode.parentNode.parentNode.nextSibling.firstChild.firstChild.firstChild.nextSibling;
				var fct = function (){ob_t24(ob_tb2, url, callback)};
				window.setTimeout(fct, 100);
			}
			else callback();
		}
	}		
	
	if(tree_selected_path!=null && (typeof(ob_select_on_collapse) != "undefined" && ob_select_on_collapse == true)){
		var s = os.src.substr((os.src.length - 8), 8);
		if ((s=="ik_l.gif")||(s=="usik.gif")||(s=="ik_r.gif")){
			var e = os.parentNode.parentNode.firstChild.nextSibling.nextSibling;
			var a = tree_selected_path.split(",");
			for (var i=0; i<a.length; i++) {
				if(a[i]==e.id && e.id !=  tree_selected_id){
					tree_node_exp_col = true;
					ob_t22(e);
					tree_node_exp_col = false;
					return;
				}
			}
		}
	}			
}

function ob_t22(ob_od, ev, multiselect) {		
	// Highlight selected node		
	if (typeof(ob_od.className) == 'undefined' && ob_od.parentNode.className == 'ob_t2') ob_od = ob_od.parentNode;
	if (ob_od.id == "") return;		
	
	// Check if the node is in the unselectable nodes list
	if(typeof(sNoSelect) != "undefined" && ("," + sNoSelect.replace(" ", "") + ",").indexOf("," + ob_od.id + ",") != -1) return;	
	
	// EVENT. Before node is selected.
	if(!ob_OnBeforeNodeSelect(ob_od.id))
	{
		return;
	}

	if (typeof (ob_expand_single) != 'undefined' && ob_expand_single && typeof(ob_tree_first_call) != 'undefined' && ob_tree_first_call)
	{
		parentNode = ob_getParentOfNode(ob_od);
		if (parentNode != null) ob_expandSingle(parentNode);
	}
	
	try
	{
		ob_nodeEnsureVisible(ob_od);
	}
	catch (e) {}

	prevSelected = document.getElementById(tree_selected_id);

    var iPrevSelected = 0;
	if (typeof(ob_tree_multiselect_enable) != 'undefined'&&ob_tree_multiselect_enable==true)
	{
	    if(multiselect == null || multiselect == false) {
		    iPrevSelected = ob_multiselect(ob_od, ev);
		}
	}
	else
	{
	    if (ob_prev_selected != null)
		    ob_prev_selected.className = "ob_t2";
		ob_sn2 = ob_od.id;		
    }

	if ((prevSelected != null)&&typeof(ob_tree_editnode_enable) != 'undefined'&&(ob_tree_editnode_enable==true)) ob_attemptEndEditing(ob_od);

	ob_prev_selected = ob_od;
	if(iPrevSelected) {
        ob_od.className = "ob_t2";
    } else {
        ob_od.className = "ob_t3";
    }

		
	if (typeof(ob_tree_editnode_enable) != 'undefined'&&ob_tree_editnode_enable==true){ob_attemptStartEditing(ob_od)};

	ob_op2 = ob_od;
	tree_selected_id = ob_od.id;	
	if(!ev || !ev.ctrlKey)
	{
	    var sTempSn2 = ob_sn2;
        sTempSn2 = "|" + ob_sn2 + "|";
        if(sTempSn2.indexOf(tree_selected_id + "|") == -1)
        {
            if(ob_sn2)
            {
                ob_sn2 += "|";
            }
            ob_sn2 += tree_selected_id;
        } 
	}
	
	var sel_id;
	if(ob_od.parentNode.parentNode.parentNode.parentNode.parentNode.className=="ob_di2")
	{
		tree_selected_path = tree_selected_id;
		tree_parent_id = "root";
	}
	else 
	{	    	    
		ob_od = ob_od.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
		tree_parent_id = ob_od.id;
		tree_selected_path = tree_parent_id+","+tree_selected_id;		
		ob_t20(ob_od);
	}        
    
	// EVENT. After node is selected.	
	
	if ((ev && ev.ctrlKey && ob_op2.className == "ob_t3") || (!ev || !ev.ctrlKey))
	{
	    // artifice for firefox
	    if(ob_lastControlSelected)
	    {
	        inited = 0;
	        ob_lastControlSelected.blur();
	        ob_lastControlSelected = "";
	    }
	    ob_OnNodeSelect(tree_selected_id);
	}
	
	// simulating the click on the links from the nodes
	// in case nodes have links and the click takes place outside the link			 		
	/*if(ob_op2.firstChild.nodeName == "A") {
		var oLink = ob_op2.firstChild;
		var iPosX, iPosY;
		if (window.event){var ev = window.event;iPosX = ev.x;iPosY = ev.y;} else { iPosX = ev.pageX; iPosY = ev.pageY;}
				
		iLinkX = calcOffsetX(oLink);
		iLinkY = calcOffsetY(oLink);						
		if((iLinkX + oLink.offsetWidth <= iPosX) || (iLinkX > iPosX) || (iLinkY > iPosY) || (iLinkY + oLink.offsetHeight <= iPosY)) {			
			window.open(oLink.href, oLink.target? oLink.target : '_self');
		}
	}*/
}

function ob_SelectedId(id) {
	ob_sids = new Array();
	var strSids = "";
	
	var oNode = document.getElementById(id);	
	oNode = ob_getParentOfNode(oNode);
	while(oNode != null) {
		if(strSids != "") {
			strSids = oNode.id + "," + strSids;
		} else {
			strSids = oNode.id;
		}
		oNode = ob_getParentOfNode(oNode);
	}
	
	if(strSids != "") {
		strSids += "," + id;
		ob_sids = strSids.split(",");
	}	
	ob_t26(ob_sids);
}

function ob_t20(e){
	// Extend all parents up and populate tree_selected_path
	
	var os = e.parentNode.firstChild.firstChild;

	if (os != null) {
		if ((typeof os != "undefined") && (os.tagName == "IMG")) {
			var lensrc = (os.src.length - 8);
			var s = os.src.substr(lensrc, 8);
			if ((s == "usik.gif") || (s == "ik_l.gif") || (s == "ik_r.gif")) {
				os.onclick();
			}
			if(e.parentNode.parentNode.parentNode.parentNode.parentNode.className=="ob_di2"){return};
			e=e.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling
			tree_selected_path=e.id+","+tree_selected_path;
			ob_t20(e);
		}
	}
}

function ob_t23(e){
	// To expand-collapse node by clicking on node text.
	// Find first image with plus-minus and call onclick()

	var os = e.parentNode.parentNode.firstChild.firstChild;

	if (os != null) {
		if ((typeof os != "undefined") && (os.tagName == "IMG")) {
			var lensrc = (os.src.length - 8);
			var s = os.src.substr(lensrc, 8);
			if ((s == "inus.gif") || (s == "usik.gif") || (s == "us_l.gif") || (s == "ik_l.gif") || (s == "us_r.gif") || (s == "ik_r.gif")) {
				os.onclick();
			}
		}
		else {
			ob_t23(e.parentNode);
		}
	}
}

function ob_t25(ob_od) 
{                    
	// When tree is first loaded - Highlight and Extend selected node.
	if(ob_od==null) {
		// alert("Selected node does not exists or its id is not unique.");
		return
	};
	var e, lensrc, s;
	e = ob_od.parentNode.firstChild.firstChild;
	if ((typeof e.src != "undefined") && (e.tagName == "IMG")) {
		s = e.src.substr((e.src.length - 8), 8);		
		if ((s == "usik.gif") || (s == "ik_l.gif") || (s == "ik_r.gif")) {
			// Comment out or delete this line if you don't want to expand selected node.
			e.onclick();
		}
	}
	ob_t22(ob_od);
}

function ob_tall(exp, id) {
	// To expand all nodes ob_tall(1)
	// To collapse all nodes ob_tall(0)
	
	if(id == null) {
	    var arrImages = document.images;
	} else {
	    oNode = document.getElementById(id);
	    oDiv = oNode.parentNode.parentNode.parentNode.parentNode;
	    var arrImages = oDiv.getElementsByTagName("IMG");
	}
	
	var i, e, lensrc, s
	for (i=0; i<arrImages.length; i++) {
		e = arrImages[i];
		lensrc = (e.src.length - 8);
		s = e.src.substr(lensrc, 8);
		if (exp == 1) {
			if ((s == "usik.gif") || (s == "ik_l.gif") || (s == "ik_r.gif")) {
				e.onclick();
			}
		}
		else
			if ((s == "inus.gif") || (s == "us_l.gif") || (s == "us_r.gif")) {
				e.onclick();
			}
	}
}

function ob_t2c(e) {
	// To check/uncheck checkboxes in children.
	if(ob_hasChildren(e.parentNode)) {
		var ob_t2in=e.parentNode.parentNode.parentNode.parentNode.nextSibling.getElementsByTagName("input");
		for (var i=0; i<ob_t2in.length; i++) {
			if (e.checked == true) {
				ob_t2in[i].checked = true;
			}
			else {
				ob_t2in[i].checked = false;
			}
		}
	}
	if(e.checked==false) {
		ob_t2_unchk_parent(e.parentNode);
	}else{
		// To check parent when all the child checkboxes are checked
		ob_t2_chk_parent(e.parentNode);
	}
}


function ob_t2_list_checked() {
	// Make string with checked checkboxes IDs.
	
	var ob_t2in,ob_t2s,ob_t2checked="";
	ob_t2in=document.getElementsByTagName("input");
	for (var i=0; i<ob_t2in.length; i++) {
		ob_t2s=ob_t2in[i].id;
		if ((ob_t2s.substr(0,4)=="chk_") && (ob_t2in[i].checked)) {
			ob_t2checked=ob_t2checked+ob_t2s+",";
		}
	}
	return ob_t2checked;
}


function ob_t2_unchk_parent(e) {
	// if checkbox unchecked, uncheck all its parents up to root.
	e = ob_getParentOfNode(e);
	if (e!=null){
		if ((e.firstChild!=null) && (e.firstChild.id) && (e.firstChild.id.substr(0,4)=="chk_")) {
			e.firstChild.checked=false;
			ob_t2_unchk_parent(e);
		}
	}
}

function ob_t2_chk_parent(e) {
	// if all child checked, check all its parents up to root.
	e = ob_getParentOfNode(e);
	if (e!=null){
		if ((e.firstChild!=null) && (e.firstChild.id) && (e.firstChild.id.substr(0,4)=="chk_")) {
			
			var ob_t2in = e.parentNode.parentNode.parentNode.nextSibling.getElementsByTagName("input");
			var fAllChked = true;
			for ( var i = 0; i < ob_t2in.length; i++ ) {
				if ( ob_t2in[i].checked == false ) {
					fAllChked = false;
					break;
				}
			}

			if ( fAllChked )
			{
				e.firstChild.checked = true;	
			}
			ob_t2_chk_parent(e);
		}
	}
}

function ob_hasChildren(ob_od)
{
	try
	{				
		var oTempParent = ob_od.parentNode.parentNode.parentNode;				
		return (oTempParent.nextSibling != null && oTempParent.nextSibling.nodeName.toLowerCase() == "table");		
	}
	catch (e)
	{
		return false;
	}
}

function ob_isExpanded(ob_od)
{
	try
	{
		imgSrc = ob_od.parentNode.firstChild.firstChild.src;
		lenSrc = imgSrc.length - 8;
		imgSrcLast = imgSrc.substr(lenSrc, 8);
		return (imgSrcLast == 'inus.gif' || imgSrcLast == 'us_l.gif' ||imgSrcLast == 'us_r.gif');
	}
	catch (e)
	{
		return false;
	}
}

function ob_nodeIsChildOf(node, parent)
{
	try
	{
		return ob_getParentOfNode(node) == parent;
	}
	catch (e)
	{
	}

	return false;
}

function ob_nodeIsSubNodeOf(node, parent)
{
	try
	{
		if (parent != null)
		{
			do
			{
				node = ob_getParentOfNode(node);
				if (node == parent) return true;
			} 
			while (node != null)
		}
	}
	catch (e)
	{
	}

	return false;
}

function ob_hasSiblings(ob_od)
{	
	try
	{
		return (ob_getChildCount(ob_getParentOfNode(ob_od)) > 1);
	}
	catch (e)
	{
		return false;
	}
}

function ob_getParentOfNode (ob_od)
{
	try
	{
		var oParent = ob_od.parentNode.parentNode.parentNode.parentNode.parentNode;
		if(oParent.className=="ob_di2") { return null }
		else 
		{
			if (ob_hasChildren(ob_od.parentNode)) return oParent.parentNode.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
			else return oParent.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
		}
	}
	catch (e)
	{
	}

	return null;
}

function ob_getChildCount (ob_od)
{
	try
	{
		if (ob_hasChildren(ob_od)){
			return ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.childNodes.length;
		}else{
			return 0;
		}
	}
	catch (e)
	{
	}

	return -1;
}

function ob_getChildAt (ob_od, index, expand)
{
	try
	{
		if (ob_od != null && ob_hasChildren(ob_od) && index >= 0)
		{
			if (!ob_isExpanded(ob_od) && expand) 
			try
			{
				ob_od.parentNode.firstChild.firstChild.onclick();
			} catch (e) {}
			return ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.childNodes[index].firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
		}
	}
	catch (e)
	{
	}

	return null;
}

function ob_getIndexOfChild (ob_od)
{
	try
	{
		nodeParent = ob_getParentOfNode (ob_od);
		childCount = ob_getChildCount (nodeParent);
		
		for (i = 0; i < childCount; i++)
			if (ob_getChildAt(nodeParent, i, false) == ob_od) return i;
	}
	catch (e)
	{
	}

	return -1;
}

function ob_getLastChildOfNode (ob_od, expand)
{
	try
	{
		var lastChild;
		if (ob_hasChildren(ob_od))
		{
			if (!ob_isExpanded(ob_od) && expand)
			{
				try
				{
					ob_od.parentNode.firstChild.firstChild.onclick();
				} catch (e) {}
			}

			lastChild = ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.lastChild.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
		}
		
		return lastChild;
	}
	catch (e)
	{
	}

	return null;
}

function ob_getFirstChildOfNode (ob_od, expand)
{
	try
	{
		firstChild = null;
		if (ob_hasChildren(ob_od))
		{			
			if (!ob_isExpanded(ob_od) && expand)
			{
				try
				{
					ob_od.parentNode.firstChild.firstChild.onclick();
				} catch (e) {}
			}

			if (ob_isExpanded(ob_od))
			{				
				var oTemp = ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.firstChild.firstChild.firstChild.firstChild.firstChild;				
				if(oTemp.nextSibling)
				{
					firstChild = oTemp.nextSibling.nextSibling;
				}
				else
				{					
					firstChild = oTemp.firstChild.childNodes[2];
				}		
			}
		}
		
		return firstChild;
	}
	catch (e)
	{
	}

	return null;
}

function ob_getNextSiblingOfNode (ob_od)
{
	try
	{
		nxtSibling = ob_od.parentNode.parentNode.parentNode.parentNode.nextSibling;
		if (nxtSibling != null)
		{
			nxtSibling = nxtSibling.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
		}
		
		return nxtSibling;
	}
	catch (e)
	{
	}

	return null;
}

function ob_getPrevSiblingOfNode (ob_od)
{
	try
	{
		prvSibling = ob_od.parentNode.parentNode.parentNode.parentNode.previousSibling;
		if (prvSibling != null)
		{
			prvSibling = prvSibling.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
		}
		
		return prvSibling;
	}
	catch (e)
	{
	}

	return null;
}

function ob_getFurthestChildOfNode (ob_od, expand)
{
	fChild = ob_od;

	while (true)
	{
		if (ob_hasChildren(fChild) && (ob_isExpanded(fChild) || expand))
		{
			if (!ob_isExpanded(ob_od) && expand)
			{
				try
				{
					ob_od.parentNode.firstChild.firstChild.onclick();
				} catch (e) {}
			}
		
			if (ob_isExpanded(fChild))
			{
				tmpChild = ob_getLastChildOfNode(fChild);
				if (tmpChild == null) break;
				else fChild = tmpChild;
			}
		}
		else break;
	}
	
	return fChild;
}

function ob_elementBelongsToTree (ob_od, ob_tree_id)
{
	try
	{
		if (ob_od.tagName.toLowerCase() == "body") return false;
		if (ob_od.className.toLowerCase() == "ob_tree") return true;

		while (ob_od.parentNode.tagName.toLowerCase() != "body")
		{
			if (ob_od.parentNode.className.toLowerCase() == "ob_tree")
				if (!ob_tree_id)
					return true;
				else
					return ob_od.parentNode.id;
			ob_od = ob_od.parentNode;
		}
		
		return false;
	}
	catch (e)
	{
	}

	return false;
}

function ob_getFirstNodeOfTree ()
{
	try
	{	    
		tree_div = document.getElementById(ob_tree_id);
		if (tree_div != null)
		{		    
			rootNode = tree_div.childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[2];
			
			if(rootNode == null || rootNode.innerHTML == "")			
			{			    			    
			    rootNode = ob_getFirstChildOfNode(rootNode);
			}			
			if (rootNode != null) return rootNode;
		}
	}
	catch (e)
	{
	}
}

function ob_getNodeUp (ob_od, expand)
{
	prevSibling = ob_getPrevSiblingOfNode(ob_od);
	if (prevSibling != null) 
	{
		prevSibling = ob_getFurthestChildOfNode(prevSibling, expand);
		return prevSibling;
	}
	else
	{
		nodeParent = ob_getParentOfNode(ob_od);
		if (nodeParent != null)
		{
			return nodeParent;
		}
	}

	return null;
}

function ob_getNodeDown (ob_od, expand)
{   
	if (ob_hasChildren(ob_od) && (ob_isExpanded(ob_od) || expand))
	{		
		nxtSibling = ob_getFirstChildOfNode(ob_od, expand);		
		return nxtSibling;
	}
	else
	{
		nxtSibling = ob_getNextSiblingOfNode(ob_od);
		if (nxtSibling != null) return nxtSibling;
		
		nodeParent = ob_od;
		do
		{
			nodeParent = ob_getParentOfNode(nodeParent);
			if (nodeParent != null)
			{
				nxtSibling = ob_getNextSiblingOfNode(nodeParent);
				if (nxtSibling != null) return nxtSibling;
			}
		} while (nodeParent != null && nodeParent.className.toLowerCase() != "ob_tree")
	}
	
	return null;
}

function ob_getExpanded (ob_od, onlyChildren)
{
	exp = '';
	var ob_iRoot = false;	
	if (!ob_od) {ob_od = ob_getFirstNodeOfTree(); ob_iRoot = true;}	
	node = ob_od;	
	
	if(node)
	{
		if(!ob_iRoot) node = ob_getNodeDown(node);	
	}
	else if(document.getElementById(ob_tree_id))
	{
		node = document.getElementById(ob_tree_id).childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[2];			
	}
	
	while (node != null && ((onlyChildren && ((ob_iRoot == true) || ob_nodeIsChildOf(node, ob_od))) || (!onlyChildren && ((ob_iRoot == true) || ob_nodeIsSubNodeOf(node, ob_od)))))
	{						
		if (ob_hasChildren(node) && ob_isExpanded(node)) exp += ((exp.length > 0) ? ',' : '') + node.id;
		node = onlyChildren ? ob_getNextSiblingOfNode(node) : ob_getNodeDown(node);			
	}	
	
	return exp;
}

function ob_getCollapsed (ob_od, onlyChildren)
{
	exp = '';
	var ob_iRoot = false;	
	if (!ob_od) {ob_od = ob_getFirstNodeOfTree(); ob_iRoot = true;}	
	node = ob_od;	
		
	if(node)
	{
		if(!ob_iRoot) node = ob_getNodeDown(node);	
	}
	else if(document.getElementById(ob_tree_id))
	{
		node = document.getElementById(ob_tree_id).childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[0].childNodes[2];			
	}
	
	while (node != null && ((onlyChildren && ((ob_iRoot == true) || ob_nodeIsChildOf(node, ob_od))) || (!onlyChildren && ((ob_iRoot == true) || ob_nodeIsSubNodeOf(node, ob_od)))))
	{						
		if (ob_hasChildren(node) && !ob_isExpanded(node)) exp += ((exp.length > 0) ? ',' : '') + node.id;		
		node = onlyChildren ? ob_getNextSiblingOfNode(node) : ob_getNodeDown(node);		
	}
	
	return exp;
}

function ob_expandSingle(ob_od)
{
	if (typeof(ob_tree_first_call) != 'undefined' && ob_tree_first_call)
	{
		ob_tree_first_call = false;
		parentNode = ob_getParentOfNode(ob_od);
		do
		{
			if (parentNode != null) ob_expandSingle (parentNode);
			else break;
			parentNode = ob_getParentOfNode(parentNode);
		} while (true);
	}
	
	parentNode = ob_getParentOfNode(ob_od);
	/*if (parentNode != null)
	{*/
		exp = ob_getExpanded(parentNode, true);		
		if (exp.length > 0)
		{
			var boxArray = exp.split(",");
			for (i = 0; i < boxArray.length; i++)
			{
				if (boxArray[i] != ob_od.id)
				{
					node = document.getElementById(boxArray[i]);
					if (node != null)
					{
						tmpOs = node.parentNode.firstChild.firstChild;
						if (tmpOs != null) tmpOs.onclick();
					}
				}
			}
		}
	//}
}

function ob_t2_Add(parentId, childId, textOrHTML, expanded, image, subTreeURL)
{		
	if (!ob_OnBeforeAddNode(parentId, childId, textOrHTML, expanded, image, subTreeURL))
		return;    
		
	var pNode = document.getElementById(parentId);
		
	if (!pNode) 
	{	   
	    alert("Parent does not exist.");
		return;		
	}	
	if(pNode)	
	{
		if (pNode.className.toLowerCase() != 'ob_t2' && pNode.className.toLowerCase() != 'ob_t3')
		{
			alert("Parent node is not a valid tree node.");
			return;
		}
	}
	
	dParent = pNode.parentNode.parentNode.parentNode.parentNode;
	    	
	if (document.getElementById(childId) != null) 
	{
		alert("An element with the specified childId already exists.");
		return;
	}		
    if (!ob_hasChildren(pNode))
    {
	    var e = dParent.firstChild.firstChild.firstChild.firstChild.firstChild;
	    e.src=ob_style+"/minus" + (ob_getLastChildOfNode(ob_getParentOfNode(pNode)) == pNode ? "_l.gif" : ".gif");
	    e.onclick=function(){ob_t21(this,'')};
		
	    e = dParent.appendChild(document.createElement("TABLE"));		    
		e.className = "ob_t2g";
		
		if (document.all)
		{			
			e.cellSpacing = "0";
		}
		else
		{			
			e.setAttribute("cellspacing", "0");
		}
		
	    e.appendChild(document.createElement("tbody"));
	    var e2=e.firstChild.appendChild(document.createElement("TR"));
	    e=e2.appendChild(document.createElement("TD"));
	    if(dParent.parentNode.lastChild!=dParent)
		    e.style.backgroundImage="url("+ob_style+"/vertical.gif)";
	    e.innerHTML="<div class=ob_d5></div>";
	    e=e2.appendChild(document.createElement("TD"));
	    e.className="ob_t5";	    
	    dParent.className = 'ob_d2b';
    }
    else
    {
		prevS = ob_getLastChildOfNode(pNode, true);
		if(prevS == null) {      
			if(pNode.parentNode.firstChild.firstChild.src.indexOf("plusik_l.gif")) {
				ob_t2_OnDynamicLoad = function() {ob_t2_Add(parentId, childId,  textOrHTML, expanded, image, subTreeURL);}    
			}
			return;
	   }
	    var oPrevSImg = prevS.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.firstChild;
	    
		if (!ob_hasChildren(prevS)) oPrevSImg.src = ob_style + "/hr.gif";
		else 
		{				
			if (!ob_isExpanded(prevS))
			{					
				oPrevSImg.src = ob_style + "/plusik.gif";										
			}
			else oPrevSImg.src = ob_style + "/minus.gif";
			prevS.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.style.backgroundImage = "url(" + ob_style + "/vertical.gif)";				
		}		    			
		
	    if(dParent.parentNode.lastChild!=dParent)
		    prevS.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.firstChild.style.backgroundImage = "url(" + ob_style + "/vertical.gif)";
		    
		oPrevSImg.parentNode.className = "ob_t6v";
    }
	
	div = document.createElement('div');
	div.className = 'ob_d2c';	
	
	sInnerHtml = '<table cellspacing="0" class="ob_t2g"><tr><td class="ob_t6"><img ' + ((subTreeURL != null) ? 'src="' + ob_style + '/plusik_l.gif" onclick="ob_t21(this, \'' + subTreeURL + '\')"' : 'src="' + ob_style + '/hr_l.gif"') + '></td>';
	sInnerHtml += '<td class="ob_t4"' + (ob_t2_showicons == false ? ' style="display:none;"' : '') + '>' + (ob_t2_showicons == true ? '<div class="ob_d4">' : '') + ((image != null && typeof(ob_icons) != 'undefined' && ob_t2_showicons == true) ? '<img src="' + ob_icons + '/' + image + '">' : '') + (ob_t2_showicons == true ? '</div>' : '') + '</td>';
	sInnerHtml += '<td id=' + childId + ' onclick="ob_t22(this, event)" class="ob_t2">' + textOrHTML + '</td></tr></table>';	
	
	div.innerHTML = sInnerHtml;
	node = div.firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling;
	node.className = 'ob_t2';
	//node.nowrap = "true";
	if (subTreeURL != null) div.innerHTML += '<table cellspacing="0" style="display: none;"><tbody><tr><td><div class=ob_d5></div></td><td class="ob_t7">Loading ...</td></tr></tbody>';	
						
	try
	{			
		if(dParent.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.innerHTML == "Loading ...")
		{										
			dParent.removeChild(dParent.childNodes[1]);
		}			
	}
	catch(ex){}	
    dParent.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.appendChild(div);
    
    if(typeof(ob_tree_dnd_enable) != "undefined" && ob_tree_dnd_enable == true)
    {
	    if(typeof(ob_t18) != 'undefined') {
			ob_t18(dParent);
	    }
	}
	
	tree_node_exp_col = true;	
	// de-select all the selected nodes before adding
	if (ob_tree_multiselect_enable) {	
        ob_unselectAllNodes();
    }
	//if (tree_selected_id != parentId && document.getElementById(parentId)) {
	if (document.getElementById(parentId)) {
	    try{ob_t22(document.getElementById(parentId));}
	    catch(ex){}
	}
	
	tree_node_exp_col = false;
		
	if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true)
	{
	    ob_t51();
	}
	
	//document.getElementById("txtDebug").value = pNode.parentNode.parentNode.parentNode.parentNode.innerHTML;
		
    ob_OnAddNode(parentId, childId, textOrHTML, expanded, image, subTreeURL);    
	return node;
}

function ob_t2_Remove(childId)
{
	ob_od = document.getElementById(childId);
	if (!ob_od) 
	{
		alert("There is no node with the specified id.");
		return;	
	}
	
	if (!ob_OnBeforeRemoveNode(childId))
		return;

	try
	{
		nodeParent = ob_getParentOfNode (ob_od);
		var lc = ob_getLastChildOfNode(nodeParent) == ob_od;
		
		if (ob_hasSiblings(ob_od))
		{
			if (lc)
			{				
				prevS = ob_getPrevSiblingOfNode(ob_od);								
				var oPrevSImg = prevS.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.firstChild.firstChild;
				if (prevS != null)
				{
					if (!ob_hasChildren(prevS)) oPrevSImg.src = ob_style + "/hr_l.gif";
					else 
					{
						if (!ob_isExpanded(prevS))
						{
							oPrevSImg.src = ob_style + "/plusik_l.gif";
						}
						else oPrevSImg.src = ob_style + "/minus_l.gif";

						oPrevSImg.style.backgroundImage = "none";						
						prevS.parentNode.parentNode.parentNode.parentNode.childNodes[1].firstChild.firstChild.firstChild.style.backgroundImage = "none";
					}
					oPrevSImg.parentNode.style.backgroundImage = 'none';
				}
			}

			ob_od.parentNode.parentNode.parentNode.parentNode.parentNode.removeChild(ob_od.parentNode.parentNode.parentNode.parentNode);
		}
		else
		{
			if (nodeParent != null)
			{
				if (lc)
					nodeParent.parentNode.parentNode.parentNode.parentNode.className = 'ob_t2c';
				e = nodeParent.parentNode.parentNode.parentNode.parentNode;
				
				if (ob_getLastChildOfNode(ob_getParentOfNode(nodeParent)) == nodeParent) 
				{
					e.firstChild.firstChild.firstChild.firstChild.style.backgroundImage = 'none';
					e.firstChild.firstChild.firstChild.firstChild.firstChild.src = ob_style + "/hr_l.gif";
				}
				else 
				{
					e.firstChild.firstChild.firstChild.firstChild.style.backgroundImage = "url(' + ob_style + '/vertical.gif)";
					e.firstChild.firstChild.firstChild.firstChild.firstChild.src = ob_style + "/hr.gif";
				}

				e.removeChild(e.firstChild.nextSibling);
				try{ob_od.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.removeChild(ob_od.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode);}catch(e){}				
			}
			else
			{
				ob_od.parentNode.parentNode.parentNode.parentNode.parentNode.removeChild(ob_od.parentNode.parentNode.parentNode.parentNode);
			}
		}
		
		if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true)
		{
		    ob_t51();
		}	
			 
		ob_OnRemoveNode(childId);
		if (nodeParent != null)
		{
			nodeParent.onclick();
		}

	}
	catch (e)
	{
	}
}

function ob_arrIdxOf(arr,obj){for(var i=0;i<arr.length;i++){if(obj==arr[i])return i};return -1;}

function ob_g_eids()
{
	try
	{
		var en = "";
		if (document.cookie != null)
		{
			var ck = document.cookie.split(";");
			for (var i=0; i < ck.length; i++)
			{
				var tmpArr = ck[i].split("=");
				if (tmpArr[0].indexOf(ob_page_location + '_ob_eids') != -1)
				{
					en = tmpArr[1];
					break;
				}
			}
		}
		return (en.length > 0) ? en.split(",") : new Array();
	}
	catch(e)
	{
		return new Array();
	}	
}

function ob_g_cids()
{
    try
	{
		var en = "";
		if (document.cookie != null)
		{
			var ck = document.cookie.split(";");
			for (var i=0; i < ck.length; i++)
			{
				var tmpArr = ck[i].split("=");
				if (tmpArr[0].indexOf(ob_page_location + '_ob_cids') != -1)
				{
					en = tmpArr[1];
					break;
				}
			}
		}
		return (en.length > 0) ? en.split(",") : new Array();
	}
	catch(e)
	{
		return new Array();
	}	
}

function ob_InitNewKeepState()
{                
	if (ob_KeepLastExpanded <= 0 && ob_KeepLastCollapsed <= 0)
		return;
	if(ob_KeepLastExpanded)
	{
	    if (!ob_sids)
	    {
			//modified to have dynamic node expand
			function recurCheck(id_list){
		        var ob_sids = id_list || ob_g_eids();
			    ob_sids.push(ob_last);
		        ob_t26(ob_sids);
		        
		        // an array of nodes have to be expanded in next call of recurCheck function
		        var tmp_sids = [];
		        
		        if(ob_sids.length)
		        {
		            //remove un-wanted node id
		            if(typeof(ob_sids[ob_sids.length-1]) == "undefined")
		                ob_sids.pop();
    		            
		            for(var i=0; i<ob_sids.length && i<ob_KeepLastExpanded;i++)
		            {
		                if(ob_sids[i] == '' || typeof(ob_sids[i]) == "undefined")
		                {
		                    continue;
		                }
		                ob_od = document.getElementById(ob_sids[i]);
    		            
		                if(ob_od==null)
		                {
		                    //obtain node id which has been yet generated by dynamic loading
		                    tmp_sids.push(ob_sids[i]);
		                    continue;
		                }
    		            
		                var e, s;
		                e = ob_od.parentNode.firstChild.firstChild;
    		           
                        if(e && e.getAttribute("onclick"))
                        {
                            var sFunctionName = e.getAttribute("onclick").toString();
                            var sSubtreeUrl = sFunctionName.substring(sFunctionName.lastIndexOf(",") + 1, sFunctionName.lastIndexOf(")"));        
                            
                            if(sSubtreeUrl.length <= 3)
                                continue;
                            if ((typeof e.src != "undefined") && (e.tagName == "IMG")) 
                            {
                                s = e.src.substr((e.src.length - 8), 8);
                                // if the current node is open, then we have to click it twice 
                                if ((s == "inus.gif") || (s == "us_l.gif") || (s == "us_r.gif"))
                                {		                                       
                                    e.onclick();
                                    e.onclick();
                                }
                                else    //else, click the node once
                                {
                                    e.onclick();
                                }
                                //end if
                            }//end if
                        }//end if
		            }//end for
    		        
	                if(tmp_sids.length > 0)
	                {
                        var t = function(){recurCheck(tmp_sids)};
                        window.setTimeout(t, 300);
                    }
                    else
                    {
                        return;
                    }
		        }//end if
		    }//end function
		    
		    //call function recurCheck()		    
		    recurCheck();
		}
	}
	if(ob_KeepLastCollapsed)
	{	    	    
        var ob_cids = ob_g_cids();                    
        if(ob_cids && ob_cids.length)
        {                  		              
            for(var i=0; i<ob_cids.length && i<ob_KeepLastCollapsed; i++)
            {                                                
                ob_od = document.getElementById(ob_cids[i]);                           
                if(ob_od==null) {		            
		            return
	            }
	            var e, lensrc, s;
	            e = ob_od.parentNode.firstChild.firstChild;	            
	            if ((typeof e.src != "undefined") && (e.tagName == "IMG")) {
		            s = e.src.substr((e.src.length - 8), 8);
		            if ((s == "inus.gif") || (s == "us_l.gif") || (s == "us_r.gif"))
		            {		            	                                        
			            e.onclick();
			        }
	            }	            
            }           
        }        
    }
}

function ob_saveNodeState( objBlockNode, NodeId, sType) {    
	// Set expiration days here
	var expiredays = 3;
	var ExpireDate = new Date ();
	ExpireDate.setTime(ExpireDate.getTime() + (expiredays * 24 * 3600 * 1000));
	
	var ob_arrExpIds = ob_g_eids();
	var ob_arrColIds = ob_g_cids();
	
	var ob_iExpPos = ob_arrIdxOf(ob_arrExpIds, NodeId);
	var ob_iColPos = ob_arrIdxOf(ob_arrColIds, NodeId);	    	       		
	
	var ob_BlockNodeDisplay = objBlockNode.style.display;
	if(ob_BlockNodeDisplay == "")
	{
	    ob_BlockNodeDisplay = "block";
	}	
	
    if((ob_BlockNodeDisplay == "block") && (ob_iExpPos != -1)) {          
	    ob_arrExpIds.splice(ob_iExpPos, 1);		
    }
    else if((ob_BlockNodeDisplay != "block") && (ob_iExpPos == -1)){
	    ob_arrExpIds.push(NodeId);
        if (ob_arrExpIds.length > ob_KeepLastExpanded) ob_arrExpIds.splice(0, 1);        
    }            	    	        
    	
	if((ob_BlockNodeDisplay != "block") && (ob_iColPos != -1)) {	    
	    ob_arrColIds.splice(ob_iColPos, 1);		
    }
    else if((ob_BlockNodeDisplay == "block") && (ob_iColPos == -1)){
	    ob_arrColIds.push(NodeId);
        if (ob_arrColIds.length > ob_KeepLastCollapsed) ob_arrColIds.splice(0, 1);        
    }	
	
	if(ob_KeepLastExpanded > 0)
	{
        var ob_sExpIds = ob_arrExpIds.join(',');
        ob_setCookie(ob_sExpIds, ExpireDate, "", "", "", ob_page_location + "_ob_eids" );
    }
    if(ob_KeepLastCollapsed > 0)
    {    
        var ob_sColIds = ob_arrColIds.join(',');
	    ob_setCookie(ob_sColIds, ExpireDate, "", "", "", ob_page_location + "_ob_cids" );	    	    
	}		
}

function ob_setCookie(name, expires, path, domain, secure, sCookieName) {
  if(!sCookieName)
  {
    sCookieName = "ob_eids";
  }
  var curCookie = sCookieName + "=" + name +
	  ((expires) ? "; expires=" + expires.toGMTString() : "") +
	  ((path) ? "; path=" + path : "") +
	  ((domain) ? "; domain=" + domain : "") +
	  ((secure) ? "; secure" : "");
  document.cookie = curCookie;
}

function ob_nodeEnsureVisible(ob_od)
{    
	var tree = document.getElementById(ob_tree_id);
	var width = tree.style.pixelWidth || parseInt(tree.style.width.replace('px',''));
	var height = tree.style.pixelHeight || parseInt(tree.style.height.replace('px', ''));
	var topPos = (ob_getTop(ob_od) - ob_getTop(tree) - tree.scrollTop);
	var bottomPos = topPos + ob_od.offsetHeight;
	var leftPos = (ob_getLeft(ob_od) - ob_getLeft(tree) - tree.scrollLeft);
	var rightPos = leftPos + ob_od.offsetWidth;
	var dif = bottomPos - height + 1;
	if (topPos < 0) tree.scrollTop += topPos;
	else if (dif > 0) tree.scrollTop += dif;
	
	dif = rightPos - width + 1;
	if (leftPos < 0) tree.scrollLeft += leftPos;
	else if (dif > 0) tree.scrollLeft += dif;

	var bodyEl = document.compatMode == 'BackCompat' ? document.body : (document.documentElement || document.body);
	
	var dif1 = (topPos + ob_getTop(tree)) - bodyEl.scrollTop;
	var dif2 = (ob_getTop(tree) + bottomPos) - (document.body.clientHeight + bodyEl.scrollTop) + 1;
	if (dif1 < 0) bodyEl.scrollTop += dif1;
	else if (dif2 > 0) bodyEl.scrollTop += dif2;
}

function ob_getLeft(obj){var pos = 0;if(obj.offsetParent){while(obj.offsetParent){pos+=obj.offsetLeft;obj=obj.offsetParent;}}else if(obj.x)pos+=obj.x;return pos;}
function ob_getTop(obj){var pos = 0;if (obj.offsetParent){while(obj.offsetParent){pos+=obj.offsetTop;obj=obj.offsetParent;}}else if(obj.y)pos+=obj.y;return pos;}

function ob_getChildrenList(ob_od)
{
    try
    {
        var n = ob_getChildCount(ob_od);        
        var list = new Array(), childNodes = n > 0 ? ob_od.parentNode.parentNode.parentNode.parentNode.firstChild.nextSibling.firstChild.firstChild.firstChild.nextSibling.childNodes : new Array();
        for(var i = 0; i < n; i++)
			list.push(childNodes[i].firstChild.firstChild.firstChild.firstChild.nextSibling.nextSibling);
        return list;
    }
    catch(e)
    {
    }
}

function ob_getTopLevelList(tree_id)
{
    try
    {
        var firstNode = ob_getFirstNodeOfTree();
		var tree_div = document.getElementById(tree_id || ob_tree_id);
		var list = new Array();
		var nextNode = firstNode || tree_div.firstChild.firstChild.firstChild.firstChild.firstChild.lastChild.parentNode.parentNode.parentNode.parentNode.nextSibling.firstChild.firstChild.firstChild.lastChild;
		while (nextNode)
        {
        	list.push(nextNode);
            nextNode = nextNode.parentNode.parentNode.parentNode.parentNode.nextSibling;
            if (nextNode) nextNode = nextNode.firstChild.firstChild.firstChild.lastChild;
		}
        return list;            
    }
    catch(e)
    {
    }
}


var ob_sLastDeletedId = '';
function ob_t2_Move(destination_id, source_id) {    
    
    var oDestinationNode = document.getElementById(destination_id);
    var oSourceNode = document.getElementById(source_id); 
    if(oDestinationNode != null) {var oDestinationDiv = oDestinationNode.parentNode.parentNode.parentNode;}
    if(oSourceNode != null) {var oSourceDiv = oSourceNode.parentNode.parentNode.parentNode.parentNode;}
    
    if(oSourceDiv != null && oDestinationDiv != null) {
        // Edited by Teo on August 22, 2011 :
        // {ctrlKey: false} instead of 'null' as the first parameter (event)
        // o_S() throws an exception when null 'event' parameter in browsers other than IE
        o_S({ ctrlKey: false }, false, null, oDestinationDiv, oSourceDiv);
        //o_S(null, false, null, oDestinationDiv, oSourceDiv);
    }
}


// function that moves the selected node up/down
// argument sDirection has to be one of the following: up/down
function ob_t2_UpDown(sDirection, sNodeId) {

    // if the passed argument doesn't match one of the up/down directions,
    // the function may not be executed any further
    if(sDirection != 'up' && sDirection != 'down') {
        return;
    }
    
    //if there isn't any selected node, the user will receive an warning
    
    if(sNodeId == null) {
        sNodeId = tree_selected_id;
    }    
    if(typeof(sNodeId) == 'undefined') {
        alert('Please select a node to move!');
        return;
    }
                    
    // gets the main div that contains the selected node
    var oSelDiv = document.getElementById(sNodeId).parentNode.parentNode.parentNode.parentNode;
    
    // gets the parent element of the main div in which the selected node is contained
    var oParentDiv = oSelDiv.parentNode;        
    
    if(sDirection == 'up') {
        // the selected node is being moved before its previous sibling
        var oTargetDiv = oSelDiv.previousSibling;   
        if(oTargetDiv != null) {
            oParentDiv.insertBefore(oSelDiv, oTargetDiv);             
            
            ob_OnMoveNodeUp(sNodeId, oTargetDiv.firstChild.firstChild.firstChild.childNodes[2].id);
        }                
    } else {
        // the next sibling of the selected node is being moved before the selected node
        var oTargetDiv = oSelDiv.nextSibling;
        if(oTargetDiv != null) {
            oParentDiv.insertBefore(oTargetDiv, oSelDiv);
            
            ob_OnMoveNodeDown(sNodeId, oTargetDiv.firstChild.firstChild.firstChild.childNodes[2].id);
        }                
    }
}

function ob_changeIcon (oNode) {
	if (oNode != null ){	
	    if(oNode.previousSibling && oNode.previousSibling.firstChild && oNode.previousSibling.firstChild.firstChild)
	    {
	        var oImg = oNode.previousSibling.firstChild.firstChild;
	    }
	    else
	    {
	        return;
	    }
	    var sOldImg = oImg.src.substr(oImg.src.lastIndexOf("/") + 1);
	    var sNewImg = "";
	    
	    switch(sOldImg)
	    {
	        case "Folder.gif":
	             if(ob_isExpanded(oNode)){sNewImg = "Folder.gif";}else{sNewImg = "Folder_o.gif";}
	            break;
	        case "Folder_o.gif":
	            if(ob_isExpanded(oNode)){sNewImg = "Folder.gif";}else{sNewImg = "Folder_o.gif";}
	            break;
	    }
	    
	    if(sNewImg)
	    {
		    oImg.src = ob_icons + "/" + sNewImg;	
		}
	}
}

function ob_t2_Reload(sNodeId) {
    if(typeof sNodeId == 'undefined') {
        return;
    }     
                      
    var oSelectedNode = document.getElementById(sNodeId);        
    var oImg = oSelectedNode.parentNode.firstChild.firstChild;    
         
    if(oImg && oImg.getAttribute("onclick"))
    {
        var sFunctionName = oImg.getAttribute("onclick").toString();
        var sSubtreeUrl = sFunctionName.substring(sFunctionName.lastIndexOf(",") + 1, sFunctionName.lastIndexOf(")"));        
    }        
    else
    {
        var sSubtreeUrl = "''";
    }
    if(sSubtreeUrl.length > 3)    
    {
        try
        {        
		    var oDiv = oImg.parentNode.parentNode.parentNode.parentNode.nextSibling.firstChild.firstChild.firstChild.nextSibling;		
		    if (oDiv.innerHTML != "Loading ...")
		    {
		        oDiv.className = "ob_t7";
			    oDiv.innerHTML = "Loading ...";
		    }					
    						
		    oImg.onclick();
            if(!ob_isExpanded(oSelectedNode))
            {
                oImg.onclick();
            }		                       
        }
        catch(e)
        {
        }   	
    }
}

function ob_t2_GetAllTextNode( oNode, withParent ){  
	var ret = "";
	var sSpacer = '\r\n';

	if ( oNode != null  )
	{
		if ( withParent == true )
		{
			if( oNode.firstChild.firstChild) {
				ret += oNode.firstChild.firstChild.nodeValue + sSpacer;
			} else {
				ret += oNode.firstChild.nodeValue + sSpacer;
			}  
		}else{
			withParent = true;
		}

		var childNode = ob_getFirstChildOfNode ( oNode );

		if ( childNode  != null ){
			ret += ob_t2_GetAllTextNode( childNode, withParent );
		}
		var nextNode = ob_getNextSiblingOfNode( oNode ); 
		
		if ( nextNode != null ){
			ret +=ob_t2_GetAllTextNode( nextNode, withParent );
		}
	}
	return ret;
}

function ob_t2_CopyToControl(oDragableDiv, copy){  
	// copy = 0 : Copy only the moving node to textarea
	// copy = 1 : Copy moving node with child nodes to textarea
	// copy = 2 : Copy only the child nodes to textarea

    if(oDragableDiv == null) {
        return;
    }
  
    iMainTop = calcOffsetY(oDragableDiv);
    iMainLeft = calcOffsetX(oDragableDiv);       
    
	if (iMainTop == 0)
	{
		iMainTop = parseFloat(oDragableDiv.style.top); 
	}
	if (iMainLeft == 0)
	{
		iMainLeft = parseFloat( oDragableDiv.style.left); 
	}
	if(show_with_children == true)
    {
		sTextContainer = oDragableDiv.firstChild.firstChild.firstChild.childNodes[2];    
	}
	else
	{
		sTextContainer = oDragableDiv.firstChild.firstChild.childNodes[2];    		
	}
    
    if(sTextContainer.firstChild.firstChild) {
        sMainText = sTextContainer.firstChild.firstChild.nodeValue;
    } else {
        sMainText = sTextContainer.firstChild.nodeValue;
    }  

    arrTextBoxes = document.getElementsByTagName('input');
    arrTextAreas = document.getElementsByTagName('textarea');        

    if(arrTextBoxes.length) {
        for(var i=0; i<arrTextBoxes.length; i++) {
            if(arrTextBoxes[i].type == 'text') {
                
                iElTop = calcOffsetY(arrTextBoxes[i]);
                iElLeft = calcOffsetX(arrTextBoxes[i]);
                iElBottom = iElTop + arrTextBoxes[i].offsetHeight;
                iElRight = iElLeft + arrTextBoxes[i].offsetWidth;
                
                if((iMainTop > iElTop && iMainTop < iElBottom) && (iMainLeft > iElLeft && iMainLeft < iElRight)) {
                    arrTextBoxes[i].value = sMainText;                    
                    return;
                }
            }
        }  
    }
    
    if(arrTextAreas.length) {
        for(var i=0; i<arrTextAreas.length; i++) {                           
            iElTop = calcOffsetY(arrTextAreas[i]);
            iElLeft = calcOffsetX(arrTextAreas[i]);
            iElBottom = iElTop + arrTextAreas[i].offsetHeight;
            iElRight = iElLeft + arrTextAreas[i].offsetWidth;
            
            if((iMainTop > iElTop && iMainTop < iElBottom) && (iMainLeft > iElLeft && iMainLeft < iElRight)) {
                var sSpacer = '';
                if(arrTextAreas[i].value != '') {
                    sSpacer = '\r\n';
                }
				try{
					if ( copy == null || copy == 0 )
					{
						// Copy only the moving node to textarea
						arrTextAreas[i].value += sSpacer + sMainText;
					}else{
						arrTextAreas[i].value += sSpacer + ob_t2_GetAllTextNode( sTextContainer, copy != null && copy == 1 );
					}
				}catch(ex){}
                return;
            }            
        }    
    }    
}

function calcOffsetX(el) {
    offset = el.offsetLeft;
    while (el.offsetParent) 
    {
        el = el.offsetParent;
        offset += el.offsetLeft;
    }
    return offset;
}
function calcOffsetY(el) {	
    offset = el.offsetTop;
    while (el.offsetParent) 
    {
        el = el.offsetParent;
        offset += el.offsetTop;
    }
    return offset;
}

function ob_t2_MoveDraggableNode(oDragDiv) {
    // gets the parent element of the main div in which the dragged node is contained    
    var oParentDiv = oDragDiv.parentNode;
    var oTargetDiv = oParentDiv.firstChild;
    
    var srcId = oDragDiv.firstChild.firstChild.firstChild.childNodes[2].id;
    var dstId = oDragDiv.parentNode.parentNode.parentNode.parentNode.parentNode.firstChild.firstChild.firstChild.childNodes[2].id;    
    
    ob_OnBeforeNodeDrop(srcId, dstId, false);
    
    if(oTargetDiv != null) {
        oParentDiv.insertBefore(oDragDiv, oTargetDiv);       
    }
    if(typeof ob_HighlightOnDnd != "undefined" && ob_HighlightOnDnd == true)
	{
		ob_t51();
	}
	ob_OnNodeDrop(srcId, dstId, false);
}

function ob_t2_UnselectNode(sNodeId)
{
    if(sNodeId == null)
    {
        if(ob_prev_selected != null)
        {            
            ob_prev_selected.className = "ob_t2";
            ob_prev_selected = null;
        }
    }
    else
    {
        var oNode = document.getElementById(sNodeId);
        if(oNode != null)
        {
            oNode.className = "ob_t2";    
            ob_prev_selected = null;        
        }
    }
}

function ob_t27(oImg)
{	
	if(oImg == null)
	{
		return;
	}	
	ob_t22(oImg.parentNode.parentNode.nextSibling);
}

function ob_generateNewId(id, html)
{	
	//alert(id + " " + html);
	var root = id;
	var counter = 0;
	if (id.indexOf("_copy") > 0)
	{
		root = id.substring(0, id.indexOf("_copy"));
		if (id.substring(id.indexOf("_copy") + 5) * 1 == id.substring(id.indexOf("_copy") + 5))
			counter = id.substring(id.indexOf("_copy") + 5)*1;
	}
	
	do
	{
		counter++;
		var newId = root + '_copy' + counter;
	}
	while (document.getElementById(newId) != null) 
	
	return newId;
}

function ob_t2_getNodeLevel(id) {		
	var oNode = document.getElementById(id);
	var iLevel = 0;
	while(oNode != null) {			
		iLevel++;
		oNode = ob_getParentOfNode(oNode);
	}
	return iLevel;
}

//-->