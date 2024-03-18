


var notiConnection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
notiConnection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});


notiConnection.on("ReceiveNotification", (notifications, connectionId) => {
    // console.log("Received notification: ", notifications);
    console.log("connection id",connectionId);
    countUnreadNotifications(notifications);
    renderNotifications(notifications);
});

notiConnection.on("TestHub", (message) => {
    // console.log("Received notification: ", notifications);
    console.log("TEst message");
});

notiConnection.on("test-noti", (notifications, connectionId) => {
    console.log("in test noti")
    console.log("Received notification: ", notifications);
    console.log("connection id",connectionId);
    countUnreadNotifications(notifications);
    renderNotifications(notifications);
});

function requestInvitation(friendId){
    console.log("FRIENDID ",friendId)
    notiConnection.invoke("HandleAddFriend",friendId).then(()=>{
        location.href = "/AddFriendPage"
    }).catch(error => {
        location.href = "/AddFriendPage"
        console.log(error.message)
    })
}

function acceptInvitation(){
    var friendshipId = document.getElementById("FriendShipId").value;
    var senderId = document.getElementById("SenderId").value;
    var receiverId = document.getElementById("ReceiverId").value;
    notiConnection.invoke("HandleAccept",friendshipId,senderId,receiverId).then(()=>{
        console.log("acp request")
        window.location.reload();
    }).catch(error => console.log(error.toString()))
}
function rejectInvitation(){
    var friendshipId = document.getElementById("FriendShipId").value;
    var senderId = document.getElementById("SenderId").value;
    var receiverId = document.getElementById("ReceiverId").value;
    notiConnection.invoke("HandleReject",friendshipId,senderId,receiverId).then(()=>{
        location.href = "/AddFriendPage"
    }).catch(error => console.log(error.toString()))
}
function renderNotifications(notifications) {
    var notificationBox = document.getElementById("notification");

    if (notifications.length === 0) {
        var message = document.createElement("div");
        message.textContent = "There are no notifications.";
        message.classList.add("notification-item", "text-muted");
        notificationBox.appendChild(message);
        return
    }
    
    notificationBox.innerHTML = ""; // Clear existing notifications
    notifications.forEach(function(notification) {
        console.log(notification)
        // Create notification item
        var notificationItem = document.createElement("div");
        notificationItem.style.marginBottom = "20px";
        notificationItem.classList.add("notification-item","d-flex","align-item-center");
        notificationItem.style.backgroundColor = notification.isRead ? "" : "#01e4e3";

        // Create avatar
        var avatar = document.createElement("img");
        avatar.classList.add("avatar","rounded-circle");
        avatar.src = notification.clientAvatar; // Avatar URL from the JSON
        avatar.alt = "Notification Avatar";

        // Create notification content
        var notificationContent = document.createElement("div");
        notificationContent.classList.add("notification-content","d-flex","flex-column");
        notificationContent.style.marginLeft = "30px";
        var description = document.createElement("h6");
        description.classList.add("description");
        description.textContent = notification.message;

        var time = document.createElement("p");
        time.classList.add("time");
        time.textContent = timeAgo(notification.sendAt); // Assuming the time is received from the server
        // Append elements
        notificationContent.appendChild(description);
        notificationContent.appendChild(time);

        notificationItem.appendChild(avatar);
        notificationItem.appendChild(notificationContent);

        notificationBox.appendChild(notificationItem);
    });
}

function showNotification(){
    var notificationBox  = document.getElementById("notification");
    var unreadCount = document.getElementById("unreadCount");
    unreadCount.style.visibility = "hidden";
    if (notificationBox.style.display === "none" || notificationBox.style.display === "") {
        notificationBox.style.display = "block";
    } else {
        notificationBox.style.display = "none";
        var notificationItems = document.getElementsByClassName("notification-item");
        [...notificationItems].forEach(function(item) {
            item.style.backgroundColor = "white";
        });
    }
    notiConnection.send("UpdateNotificationStatus").then(()=>{console.log("Read Noti")}).catch(function (error) {
        console.error(Error)
    });
    
}
function countUnreadNotifications(notifications) {
    var unreadCount = document.getElementById("unreadCount");
    var x = notifications.reduce((count, notification) => !notification.isRead ? count + 1 : count, 0);
    unreadCount.innerHTML = x !== 0 ? x : '';
}
function timeAgo(dateString) {
    // Convert the date string to a Date object
    var date = new Date(dateString);

    // Get the current time
    var now = new Date();

    // Calculate the difference in milliseconds between the current time and the provided date
    var difference = now - date;

    // Define time intervals in milliseconds
    var minute = 60 * 1000;
    var hour = minute * 60;
    var day = hour * 24;
    var week = day * 7;
    var month = day * 30;
    var year = day * 365;

    // Determine the appropriate time interval
    if (difference < minute) {
        return 'just now';
    } else if (difference < hour) {
        return Math.floor(difference / minute) + ' minutes ago';
    } else if (difference < day) {
        return Math.floor(difference / hour) + ' hours ago';
    } else if (difference < week) {
        return Math.floor(difference / day) + ' days ago';
    } else if (difference < month) {
        return Math.floor(difference / week) + ' weeks ago';
    } else if (difference < year) {
        return Math.floor(difference / month) + ' months ago';
    } else {
        return Math.floor(difference / year) + ' years ago';
    }
}

