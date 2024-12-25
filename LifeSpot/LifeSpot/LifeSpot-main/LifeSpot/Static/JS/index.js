function filter(inputParseFuncction) {
    let elements = document.getElementsByClassName('video-container');

    for (let i = 0; i <= elements.length; i++) {
        let videoText = elements[i].querySelector('h3').innerText;
        if (!videoText.toLowerCase().includes(inputParseFuncction.toLowerCase())) {
            elements[i].style.display = 'none';
        } else {
            elements[i].style.display = 'inline-block';
        }
    }
}
// создадим объект Map для хранения сессии
//let session = {
//    'startDate' : new Date(),
//    'userAgent' : window.navigator.userAgent,
//    'userAge' : prompt("Пожалуйста, введите ваш возраст")
//};

function handleSession() {
    if (window.sessionStorage.getItem('startDate') == null) {
        window.sessionStorage.setItem('startDate', new Date().toLocaleString())
    }

    if (window.sessionStorage.getItem('userAgent') == null) {
        window.sessionStorage.setItem('userAgent', window.navigator.userAgent)
    }

    if (window.sessionStorage.getItem('userAge') == null) {
        let input = prompt('Pls, write your age:')
        window.sessionStorage.setItem('userAge', input)

        checker(true)
    } else {
        checker(false)
    }

    sessionLog()
}

let checker = function (newVisit) {
    if (window.sessionStorage.getItem('userAge') >= 18) {
        if (newVisit) {
            alert("Приветствуем на LifeSpot! " + '\n' + "Текущее время: " + new Date().toLocaleString());
        }
    }
    else {
        alert("Наши трансляции не предназначены для лиц моложе 18 лет. Вы будете перенаправлены");
        window.location.href = "http://www.google.com"
    }
}

let sessionLog = function logSession() {
    console.log('Начало сессии: ' + window.sessionStorage.getItem('startDate'))
    console.log('Даныне клиента: ' + window.sessionStorage.getItem('userAgent'))
    console.log('Возраст пользователя: : ' + window.sessionStorage.getItem('userAge'))
}