﻿var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

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
        var chatHeader = document.getElementById("chat-title");
        var messagesList = document.getElementById("messages");
        chatHeader.innerHTML = "Please choose chat to start conversation!";
        messagesList.innerHTML = ""
        const button = document.getElementById("sendButton").style.display = 'none';

        var inputMes = document.getElementById("messageInput").style.display = 'none';

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

    document.getElementById("sendButton").style.display = 'block';
    document.getElementById("messageInput").style.display = 'block';
    // Append the elements to construct the HTML structure
    chatHeader.appendChild(img);
    chatHeader.appendChild(span);

    if (chatBox.chatBoxType.toLowerCase() === "group" && !chatBox.isLeft) {

        var buttonEdit = document.createElement("button");
        buttonEdit.className = "btn btn-primary";
        var iEdit = document.createElement("i");
        iEdit.className = "fas fa-pen";
        buttonEdit.appendChild(iEdit);
        chatHeader.appendChild(buttonEdit);


        //add person to group chat

        var buttonAdd = document.createElement("button");
        buttonAdd.className = "btn btn-primary";
        var iAdd = document.createElement("i");
        iAdd.className = "fas fa-user-plus";
        buttonAdd.appendChild(iAdd);
        chatHeader.appendChild(buttonAdd);
        buttonAdd.addEventListener("click", function () {
            connection.invoke("GetFriendsListNotInChat", chatBox.chatBoxId).catch(function (err) {
                return console.error(err.toString());
            });

        });
        //out group chat
        var button = document.createElement("button");
        button.className = "btn btn-primary";
        var i = document.createElement("i");
        i.className = "fas fa-sign-out-alt";
        button.appendChild(i);
        chatHeader.appendChild(button);
        button.addEventListener("click", function () {
            connection.invoke("ExitGroupChat", chatBox.chatBoxId).catch(function (err) {
                return console.error(err.toString());
            });
        });

        //modify name

        buttonEdit.addEventListener("click", function () {
            var input = document.createElement("input");
            input.type = "text";
            input.className = "fs-5 fw-semibold";
            console.log('debug in change name ' + chatBox.chatBoxName);
            input.value = span.innerHTML;
            chatHeader.replaceChild(input, span);

            // Add event listener to handle blur event (when input loses focus)
            input.addEventListener("blur", function () {
                // Replace the input element with a new span element
                if (input.value !== "") {
                    if (input.value.length > 50) {
                        alert("Name of chat box is too long");
                        return;
                    }
                    if (input.value !== span.innerHTML {
                        connection.invoke("ChangeChatBoxName", chatBox.chatBoxId, input.value).catch(function (err) {
                            return console.error(err.toString());
                        });
                        span.innerHTML = input.value;
                    }

                    chatHeader.replaceChild(span, input);
                }
                buttonEdit.style.display = "block";
            });

            // Hide the button
            buttonEdit.style.display = "none";

        });
    }

    connection.invoke("GetMessages", chatBox.chatBoxId).then(function (messages) {
        // Handle received messages
        displayMessages(messages);
    }).catch(function (err) {
        return console.error(err.toString());
    });

    if (!chatBox.isLeft) {
        const button = document.querySelector('#sendButton button');
        button.disabled = false;
        var inputMes = document.getElementById("messageInput");
        inputMes.value = '';
        inputMes.style.color = 'black';
        inputMes.disabled = false;
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
    } else {
        const button = document.querySelector('#sendButton button');
        // Disable the button
        button.disabled = true;
        var inputMes = document.getElementById("messageInput");
        if (chatBox.chatBoxType.toLowerCase() === "group") {
            inputMes.value = 'You are currently not a member of this group';
        } else {
            inputMes.value = 'You are currently not friends. Add friend to send messages';
        }
        inputMes.style.color = 'red';
        inputMes.disabled = true;
        console.log('diablebuton');
    }
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
    console.log(message.messageType);
    if (message.messageType == "Notification") {
        console.log('notification');
        li.innerHTML = `<p class="notification" style="align-items:center; color:darkblue; text-align:center">
                                ${message.content}</p>`;

    } else {
        message.isYourOwnMess && li.classList.add('self-msg');
        console.log('message');
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
    }

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

connection.on("showFriendsListModal", function (friends, chatBoxId) {
    // Here, you can create a modal or dropdown list to display the list of friends
    // Populate the modal or dropdown list with the fetched data
    // For example, you can create a Bootstrap modal:
    var modal = document.createElement('div');
    modal.className = 'modal';
    modal.innerHTML = `
                                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Select Friends</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body">
                                    <form id="friendCheckboxForm">
                                        <ul class="list-group">
                                            ${friends.map(friend => `
                                                <li class="list-group-item">
                                                    <input type="checkbox" name="selectedFriends" value="${friend.id}" id="friend-${friend.id}">
                                                    <label for="friend-${friend.id}">${friend.name}</label>
                                                </li>
                                            `).join('')}
                                        </ul>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                    <button type="button" class="btn btn-primary">Add Selected</button>
                                </div>
                            </div>
                        </div>

                                `;
    document.body.appendChild(modal);

    // Initialize the Bootstrap modal
    var modalInstance = new bootstrap.Modal(modal);
    modalInstance.show();

    // Handle the "Add Selected" button click event
    modal.querySelector('.btn-primary').addEventListener('click', function () {
        connection.invoke("AddPeopleToChatBox", getSelectedFriendsFromModal(), chatBoxId).catch(function (err) {
            return console.error(err.toString());
        });
        modalInstance.hide();
    });

    // Handle the modal dismissal event
    modal.addEventListener('hidden.bs.modal', function () {
        // Remove the modal from the DOM when it's closed
        modal.remove();
    });
});

function getSelectedFriendsFromModal() {
    // Get the form element containing the checkboxes
    var form = document.getElementById('friendCheckboxForm');

    // Initialize an array to store the IDs of selected friends
    var selectedFriends = [];

    // Iterate over each checkbox in the form
    var checkboxes = form.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(function (checkbox) {
        // Check if the checkbox is checked
        if (checkbox.checked) {
            // If checked, add the value (friend ID) to the selectedFriends array
            selectedFriends.push(checkbox.value);
        }
    });

    // Return the array of selected friend IDs
    return selectedFriends;
}