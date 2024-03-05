$(()=>{
    var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    connection.start();
    
    connection.on("LoadNotification",function (){
        location.href = '/ChatPage'
    })
    connection.start().catch(function(error) {
        return console.error(error.toString());
    })
    
    
})