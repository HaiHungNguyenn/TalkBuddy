﻿
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

connection.on("ReceiveMessage", function (user, message) {
    console.log('username is', user);
    document.getElementById("messages").appendChild(convertMessageToListItem(message));
    scrollMessagesToBottom();
});

connection.start().then(function () {
    console.log("SignalR Connected");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("InitializeChat", function (chatBoxes) {

    document.getElementById("chatBoxtList").innerHTML = "";
    chatBoxes.forEach(function (chatBox) {
        var li = document.createElement("li");

        console.log(chatBox.ChatBoxName);
        // Create the image element
        var img = document.createElement("img");
        img.src = chatBox.chatBoxAvatar ?? "/default-avatar.png";
        img.className = "avatar";

        // Create the span element for the text
        var span = document.createElement("span");
        span.className = "fs-5 fw-semibold";
        span.innerHTML = chatBox.chatBoxName;

        // Append the elements to construct the HTML structure

        li.appendChild(img);
        li.appendChild(span);
        li.addEventListener("click", function () {
            requestMessagesForChatBox(chatBox);
        });

        // Append the list item to the chat box list

        document.getElementById("chatBoxtList").appendChild(li);
    });
});


function sendButtonClickHandler(event) {
    var chatBoxId = document.getElementById("sendButton").getAttribute("data-chatBoxId");
    var message = document.getElementById("messageInput").value;

    if (message !== "") { // Check if message is not empty
        connection.invoke("SendMessage", chatBoxId, message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    document.getElementById("messageInput").value = "";
    event.preventDefault();
}

//search for chatbox
function searchChatButtonClickHandler(event) {

    var searchString = document.getElementById("searchChatInput").value;
    connection.invoke("SearchByChatBoxName", searchString).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("searchChatInput").value = "";
    event.preventDefault();
}

function requestMessagesForChatBox(chatBox) {

    var chatHeader = document.getElementById("chat-title");
    chatHeader.innerHTML = "";
    var img = document.createElement("img");
    img.src = chatBox.chatBoxAvatar ?? "/default-avatar.png";
    img.className = "avatar show-clients";
    // Create the span element for the text
    var span = document.createElement("span");
    span.className = "fs-5 fw-semibold";
    span.innerHTML = chatBox.chatBoxName;

    // Append the elements to construct the HTML structure
    chatHeader.appendChild(img);
    chatHeader.appendChild(span);
    connection.invoke("GetMessages", chatBox.chatBoxId).then(function (messages) {
        // Handle received messages
        displayMessages(messages);
    }).catch(function (err) {
        return console.error(err.toString());
    });
    // Remove the existing click event listener for the send button
    document.getElementById("sendButton").removeEventListener("click", sendButtonClickHandler);

    // Add a new click event listener for the send button
    document.getElementById("sendButton").addEventListener("click", sendButtonClickHandler);

    // Set the chatBoxId as a data attribute on the sendButton element
    document.getElementById("sendButton").setAttribute("data-chatBoxId", chatBox.chatBoxId);

    //Add click event to show clients in chat box
    const showClient = document.getElementsByClassName("show-clients")[0];
    const clientsDialog = document.getElementById("show-clients-dialog");
    showClient.addEventListener("click", () => {
        //check type of chat box
        console.log(chatBox);
        if (chatBox.chatBoxType == "TwoPerson") {
            window.location.href = `/Profile?id=${chatBox.clientId}`
        }
        else {
            clientsDialog.showModal();
            connection.invoke("GetClientsOfChatBox", chatBox.chatBoxId).catch(function (err) {
                return console.error(err.toString());
            });
        }
    });
}

// Function to display messages on the UI
function displayMessages(messages) {
    var messagesList = document.getElementById("messages");
    messagesList.innerHTML = ""; // Clear previous messages

    // I created a function to render a single message instead of a list of messages
    // which may be easier to reuse
    messages.forEach(msg => messagesList.append(convertMessageToListItem(msg)));
    scrollMessagesToBottom();
}

function convertMessageToListItem(message) {
    // Create a new list item for each message
    const li = document.createElement("li");

    message.isYourOwnMess && li.classList.add('self-msg');

    // add avatar, the image is a placeholder, replace the image's source with client's avatar
    const imgAvatar = document.createElement("img");
    imgAvatar.src = message.senderAvatar || '/default-avatar.png';
    imgAvatar.alt = "User avatar";
    imgAvatar.height = 40;
    imgAvatar.width = 40;
    imgAvatar.classList.add("rounded-circle", "chat-client-avatar");

    const divAvatarContainer = document.createElement('div');
    if (!message.isYourOwnMess) {
        var avatarId = uuidv4();
        imgAvatar.addEventListener('mouseenter', () => {
            const divClientName = document.getElementById(avatarId);
            divClientName.style.visibility = 'visible';
        });
        imgAvatar.addEventListener('mouseleave', () => {
            const divClientName = document.getElementById(avatarId);
            divClientName.style.visibility = 'hidden';
        });

        divAvatarContainer.innerHTML = `
    <span class="position-absolute chat-client-name" id=${avatarId}>${message.senderName}</span>
    `;
    }
    divAvatarContainer.appendChild(imgAvatar);
    divAvatarContainer.classList.add("position-relative");

    const spanMessageContent = document.createElement("span");
    spanMessageContent.textContent = message.content;

    const smallTime = document.createElement("small");
    smallTime.textContent = new Date(message.sentDate).toLocaleTimeString();
    smallTime.classList.add("align-self-end");

    // this element should be a div, but I have to rewrite my css if I do so I decided to keep it as a span
    const spanContainer = document.createElement("span");
    spanContainer.classList.add("d-flex", "flex-column");
    spanContainer.appendChild(spanMessageContent);
    spanContainer.appendChild(smallTime);

    if (message.isYourOwnMess) {
        // message to the left, avatar to the right
        li.appendChild(spanContainer);
        //li.appendChild(imgAvatar);
        li.appendChild(divAvatarContainer);
    } else {
        // message to the right, avatar to the left
        li.appendChild(divAvatarContainer);
        //li.appendChild(imgAvatar);
        li.appendChild(spanContainer);
    }

    li.classList.add("d-flex", message.isYourOwnMess ? "justify-content-end" : "justify-content-start", "align-items-center", "gap-2");
    return li;
}

//show clients members for group chatbox
connection.on("ShowClientsOfChatBox", (clients) => {
    console.log(clients);
    displayClients(clients)
})
function displayClients(clients) {
    const dialogBody = document.getElementsByClassName("modal-body-dialog")[0];
    dialogBody.innerHTML = "";
    clients.forEach((client) => {
        dialogBody.innerHTML += `
            <div class="client-row row">
                <a href="/Profile?id=${client.id}" class="col-3"><img class="client-avatar-dialog rounded-circle" src="${client.profilePicture ?? '/default-avatar.png'}"/></a>
                <span class="client-name-dialog col-7">${client.name}</span>
            </div>
        `})
}

const scrollMessagesToBottom = () => {
    const ulMessages = document.getElementById('messages');
    ulMessages.scrollTop = ulMessages.scrollHeight;
}