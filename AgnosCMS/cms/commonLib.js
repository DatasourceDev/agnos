//Trim Function

function trim(inputString) {
   if (typeof inputString != "string") { return inputString; }
   var retValue = inputString;
   var ch = retValue.substring(0, 1);
   while (ch == " ") { // Check for spaces at the beginning of the string
      retValue = retValue.substring(1, retValue.length);
      ch = retValue.substring(0, 1);
   }
   ch = retValue.substring(retValue.length-1, retValue.length);
   while (ch == " ") { // Check for spaces at the end of the string
      retValue = retValue.substring(0, retValue.length-1);
      ch = retValue.substring(retValue.length-1, retValue.length);
   }
   while (retValue.indexOf("  ") != -1) { // Note that there are two spaces in the string - look for multiple spaces within the string
      retValue = retValue.substring(0, retValue.indexOf("  ")) + retValue.substring(retValue.indexOf("  ")+1, retValue.length); // Again, there are two spaces in each of the strings
   }
   return retValue; 
}

//Validate Field Function

function MM_findObj(n, d) 
{ //v4.01
  var p,i,x;  
  if(!d) d=document; 
  if((p=n.indexOf("?"))>0&&parent.frames.length) 
  {
    d=parent.frames[n.substring(p+1)].document; 
	n=n.substring(0,p);
  }
  if(!(x=d[n])&&d.all) 
    x=d.all[n]; 
  
  for (i=0;!x&&i<d.forms.length;i++) 
    x=d.forms[i][n];
	
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) 
	x=MM_findObj(n,d.layers[i].document);
	
  if(!x && d.getElementById) 
    x=d.getElementById(n); 
	
  return x;
}

function MM_validateForm() 
{ //v4.0
  var i,p,q,nm,test,name,num,min,max,errors='',args=MM_validateForm.arguments;
  
  errCheck = 0;
  
  for (i=0; i<(args.length-2); i+=3) 
  { 
    test=args[i+2]; 
	name=args[i+1];
	val=MM_findObj(args[i]);
    if (val) 
	{ 
	 //nm=val.name; 
	 nm=name + ' field';
	 if ((val=trim(val.value))!="") 
	 {
      if (test.indexOf('isEmail')!=-1) 
	  { 
	    p=val.indexOf('@');
        if (p<1 || p==(val.length-1))
        { 
		  errors+='- '+nm+' must contain an e-mail address.\n';
		  if (errCheck==0)
	        {
			  errVal = MM_findObj(args[i]);
			  errCheck=1;
			}
		}
      } 
      else if (test.indexOf('isDate')!=-1)
      {
		if (isDate(val) == false)
		{
			errors+='- '+nm+' must be in (DD/MM/YYYY) format.\n';
			if (errCheck==0)
	        {
			  errVal = MM_findObj(args[i]);
			  errCheck=1;
			}
		}
      }
	  else if (test!='R') 
	  { 
	    num = parseFloat(val);
        if (isNaN(val))
        {
		  errors+='- '+nm+' must contain a number.\n';
		  if (errCheck==0)
	        {
			  errVal = MM_findObj(args[i]);
			  errCheck=1;
			}
		}
        if (test.indexOf('inRange') != -1) 
		{ 
		  p=test.indexOf(':');
          min=test.substring(8,p); 
		  max=test.substring(p+1);
          if (num<min || max<num) 
          {
		    errors+='- '+nm+' must contain a number between '+min+' and '+max+'.\n';
		    if (errCheck==0)
	        {
			  errVal = MM_findObj(args[i]);
			  errCheck=1;
			}
		  }
        } 
	  } 
	}
	else if (test.charAt(0) == 'R') 
	{
	  errors += '- '+nm+' is required.\n'; 
	  if (errCheck==0)
	  {
		errVal = MM_findObj(args[i]);
		errCheck=1;
	  }
	}
   }
  } 
  if (errors) alert('The following error(s) occurred:\n'+errors);
  document.MM_returnValue = (errors == '');
  
  if (errCheck==1) 
  {
	if (errVal.isTextEdit == true)
	{
		errVal.select();
	}
	else
	{
		errVal.focus();
	}
  }
}

function isInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(dtStr){
	var dtCh= "/";
	
	var daysInMonth = DaysArray(12)
	var pos1=dtStr.indexOf(dtCh)
	var pos2=dtStr.indexOf(dtCh,pos1+1)
	var strDay=dtStr.substring(0,pos1)
	var strMonth=dtStr.substring(pos1+1,pos2)
	var strYear=dtStr.substring(pos2+1)
	strYr=strYear
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	month=parseInt(strMonth)
	day=parseInt(strDay)
	year=parseInt(strYr)
	if (pos1==-1 || pos2==-1){
		return false
	}
	if (strMonth.length<1 || month<1 || month>12){
		return false
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		return false
	}
	if (strYear.length != 4 || year==0){
		return false
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		return false
	}
return true
}