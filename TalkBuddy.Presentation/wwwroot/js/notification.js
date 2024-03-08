$(()=>{
    // var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    // connection.start().then(function () {
    //     console.log("Connection started");
    // }).catch(function (err) {
    //     return console.error(err.toString());
    // });
    //
    // connection.on("ReceiveNotification", (notifications, connectionId) => {
    //     console.log("Received notification: ", notifications);
    //     renderNotifications(notifications);
    // });
    //
    // function renderNotifications(notifications) {
    //     $(".notification-item").remove(); // Clear previous notifications
    //     notifications.forEach(notification => {
    //         const $notificationItem = $("<div>").addClass("notification-item");
    //         const $avatar = $("<img>").addClass("avatar").attr("alt", "Notification Avatar");
    //         $avatar.attr("src", notification.sender.profileImage);
    //         const $notificationContent = $("<div>").addClass("notification-content");
    //         const $description = $("<p>").addClass("description").text(notification.message);
    //         const $time = $("<p>").addClass("time").text(formatTime(notification.sentAt));
    //         $notificationContent.append($description, $time);
    //         $notificationItem.append($avatar, $notificationContent);
    //         $(".notification-box").append($notificationItem);
    //     });
    // }
    //
    //
    // function formatTime(dateTime) {
    //     const formattedDateTime = new Date(dateTime).toLocaleString();
    //     return formattedDateTime;
    // }
    //
    // function acceptInvitation(){
    //     var friendshipId = document.getElementById("FriendShipId").value;
    //     var senderId = document.getElementById("SenderId").value;
    //     var receiverId = document.getElementById("ReceiverId").value;
    //     connection.invoke("HandleAccept",friendshipId,senderId,receiverId)
    //     location.href="/AddFriendPage"
    // }
    // function rejecrInvitation(){
    //     var friendshipId = document.getElementById("FriendShipId").value;
    //     var senderId = document.getElementById("SenderId").value;
    //     var receiverId = document.getElementById("ReceiverId").value;
    //     connection.invoke("HandleReject",friendshipId,senderId,receiverId)
    //     location.href="/AddFriendPage"
    // }
    
})
