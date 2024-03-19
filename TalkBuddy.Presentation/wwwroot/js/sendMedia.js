document.getElementById('media').addEventListener('change', async () => {
    var mediaInput = document.getElementById('media');
    const url = await GetImageString(mediaInput);
    var mediaContainer = document.getElementById('mediaContainer');
    if (!mediaContainer) {
        mediaContainer = document.createElement('div');
        mediaContainer.id = 'mediaContainer';
        mediaContainer.style.position = 'absolute';
        mediaContainer.style.left = '70px';
        mediaContainer.style.height = '3.75rem';

        mediaContainer.innerHTML = `
        <ul id='mediaList'>
            <li id='${url}'>
                <img src="${url}" class="media-image" />
                <img src="/x-mark.png" alt="remove" class="image-removal" onclick='removeImage("${url}")' />
            </li>
        </ul>`;

        document.getElementById('chat-footer').appendChild(mediaContainer);
        document.getElementById('messageInput').disabled = true;
        document.getElementById('messageInput').placeholder = '';
    } else {
        const media = document.createElement('li');
        media.id = url;
        media.innerHTML = `
        <img src="${url}" class="media-image" />
        <img src="/x-mark.png" alt="remove" class="image-removal" onclick='removeImage("${url}")' />`;

        document.getElementById('mediaList').appendChild(media);
    }
});

function chooseMedia() {
    document.getElementById('media').click();
}

function isSendingMedia() {
    return !!document.getElementById('mediaContainer');
}

function removeImage(id) {
    document.getElementById(id).remove();
    if (document.getElementById('mediaList').childElementCount == 0) {
        document.getElementById('messageInput').placeholder = 'Type your message here';
        document.getElementById('messageInput').disabled = false;
        document.getElementById('mediaContainer').remove();
    }
}