function filter() {
    let inputString = document.getElementsByTagName('input')[0].value.toLowerCase();
    let elements = document.getElementsByClassName('video-container');

    for (let i = 0; i <= elements.length; i++) {
        let videoText = elements[i].querySelector('h3').innerText;
        if (!videoText.toLowerCase().includes(inputString.toLowerCase())) {
            elements[i].style.display = 'none';
        } else {
            elements[i].style.display = 'inline-block';
        }
    }
}