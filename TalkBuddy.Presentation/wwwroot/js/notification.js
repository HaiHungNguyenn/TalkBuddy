$(()=>{
    var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    connection.start().then(function () {
        console.log("Connection started");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveNotification", function (notifications, connectionId) {
        console.log("reach here");
        console.log("Received notification: " + notifications);
    });
    function acceptInvitation(){
        var friendshipId = document.getElementById("FriendShipId").value;
        var senderId = document.getElementById("SenderId").value;
        var receiverId = document.getElementById("ReceiverId").value;
        connection.invoke("HandleAccept",friendshipId,senderId,receiverId)
        location.href="/AddFriendPage"
    }
    function rejecrInvitation(){
        var friendshipId = document.getElementById("FriendShipId").value;
        var senderId = document.getElementById("SenderId").value;
        var receiverId = document.getElementById("ReceiverId").value;
        connection.invoke("HandleReject",friendshipId,senderId,receiverId)
        location.href="/AddFriendPage"
    }
})