var popUp;

function SetControlValue(controlID, newDate, isPostBack)
{
    popUp.close();
    document.forms[0].elements[controlID].value=newDate;
    __doPostBack(controlID,'');
}

function OpenPopupPage (pageUrl, controlID, isPostBack)
{
    popUp=window.open(pageUrl+'?controlID='+controlID+'&isPostBack='+ isPostBack,'popupcal', 'width=250,height=300,left=200,top=250'); 
}
		                         
