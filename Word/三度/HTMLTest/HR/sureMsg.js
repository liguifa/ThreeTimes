$(document).ready(function () {
    $("#aa").accordion({    
        animate:true,
        width:233,
        height:300,
        fit:false,
        border:false,
        multiple:false，
        selected:4
        
    });
    $("#easyui-layout").layout('collapse','south');  
    $("#easyui-layout").layout('collapse','east');
   

})
