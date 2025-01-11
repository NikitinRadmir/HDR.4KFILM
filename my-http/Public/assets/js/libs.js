document.addEventListener('DOMContentLoaded', function () {
    const btnMenu = document.querySelector('.btn-menu');
    const menuOverlay = document.getElementById('menu-overlay');
    const menuContent = menuOverlay.querySelector('.menu-content');
    const loginButton = document.querySelector('.button.show-login');
    const addToBookmarksButton = document.getElementById('addToBookmarksButton');
    const mainLink = document.getElementById('mainLink');
    const popupOverlayLoginButton = document.getElementById('popupOverlayLoginButton');
    const adminPanelButton = document.getElementById('adminPanelButton');
    const switchAccountButton = document.getElementById('switchAccountButton');
    const stayOnSiteButton = document.getElementById('stayOnSiteButton');
    const mainLinkNav = document.getElementById('mainLinkNav');

    const loginButtonEnter = document.querySelector('.btn.enter-btn');
    const popupOverlayLoginButtonEnter = document.getElementById('popupOverlayEnterBtn');
    const adminPanelButtonEnter = document.getElementById('adminPanelBtn');
    const switchAccountButtonEnter = document.getElementById('switchAccountBtn');
    const stayOnSiteButtonEnter = document.getElementById('stayOnSiteBtn');

    if (!btnMenu) {
        console.error('Element with class "btn-menu" not found');
    }

    if (!menuOverlay) {
        console.error('Element with id "menu-overlay" not found');
    }

    if (!menuContent) {
        console.error('Element with class "menu-content" not found');
    }

    if (!loginButton) {
        console.error('Element with class "button show-login" not found');
    }

    btnMenu.addEventListener('click', function () {
        menuOverlay.style.display = 'block';
        setTimeout(function () {
            menuContent.style.transform = 'translateX(0)';
        }, 10);
    });

    menuOverlay.addEventListener('click', function (event) {
        if (event.target === menuOverlay) {
            menuContent.style.transform = 'translateX(-100%)';
            setTimeout(function () {
                menuOverlay.style.display = 'none';
            }, 300);
        }
    });

    let lastScrollTop = 0;
    const nav = document.querySelector('.shap');

    if (!nav) {
        console.error('Element with class "shap" not found');
    }

    window.addEventListener('scroll', function () {
        let scrollTop = window.pageYOffset || document.documentElement.scrollTop;

        if (scrollTop > lastScrollTop) {
            // Scrolling down
            nav.classList.add('hidden');
        } else {
            // Scrolling up
            nav.classList.remove('hidden');
        }
        lastScrollTop = scrollTop <= 0 ? 0 : scrollTop; // For Mobile or negative scrolling
    }, false);

    // Функция для обновления текста кнопки после авторизации
    function updateLoginButton(username) {
        if (username) {
            loginButton.textContent = username;
            loginButtonEnter.textContent = username;
        }
    }

    // Функция для управления видимостью блока
    function updateBookmarksButtonVisibility(isAuthorized) {
        if (isAuthorized) {
            addToBookmarksButton.style.display = 'inline-block';
        } else {
            addToBookmarksButton.style.display = 'none';
        }
    }

    // Проверка авторизации при загрузке страницы
    function checkAuthorization() {
        // Здесь вы можете сделать AJAX-запрос к серверу для проверки авторизации
        // Например, используя fetch или XMLHttpRequest
        fetch('/auth/check')
            .then(response => response.json())
            .then(data => {
                if (data.isAuthorized) {
                    updateLoginButton(data.username);
                    updateBookmarksButtonVisibility(true);
                } else {
                    updateBookmarksButtonVisibility(false);
                }
            })
            .catch(error => {
                console.error('Error checking authorization:', error);
                updateBookmarksButtonVisibility(false);
            });
    }

    // Проверка авторизации при загрузке страницы
    checkAuthorization();

    // Обработчик для кнопок входа
    function handleLoginButtonClick(event, popupOverlay) {
        console.log('Login button clicked');
        // Проверка авторизации
        fetch('/auth/check')
            .then(response => response.json())
            .then(data => {
                if (data.isAuthorized) {
                    // Показать всплывающее окно
                    popupOverlay.style.display = 'block';
                } else {
                    // Перенаправляем на страницу auth/login
                    window.location.href = 'auth/login';
                }
            })
            .catch(error => {
                console.error('Error checking authorization:', error);
            });
    }

    loginButton.addEventListener('click', function (event) {
        handleLoginButtonClick(event, popupOverlayLoginButton);
    });

    loginButtonEnter.addEventListener('click', function (event) {
        handleLoginButtonClick(event, popupOverlayLoginButtonEnter);
    });

    // Обработчики для кнопок всплывающего окна
    adminPanelButton.addEventListener('click', function () {
        window.location.href = 'admlogin';
    });

    switchAccountButton.addEventListener('click', function () {
        window.location.href = 'auth/login';
    });

    stayOnSiteButton.addEventListener('click', function () {
        popupOverlayLoginButton.style.display = 'none';
    });

    var movieElements = document.querySelectorAll('[data-movie-id]');

    movieElements.forEach(function (element) {
        var movieId = element.getAttribute('data-movie-id');
        var linkElement = element.querySelector('a');

        // Проверяем, начинается ли HtmlPageUrl с "/film?id="
        if (!linkElement.href.startsWith('/film?id=')) {
            // Если нет, заменяем ссылку на GET-запрос
            linkElement.href = '/film?id=' + movieId;
        }
    });

    mainLink.addEventListener('click', function (event) {
        event.preventDefault();
        window.location.href = '/main';
    });
    mainLinkNav.addEventListener('click', function (event) {
        event.preventDefault();
        window.location.href = '/main';
    });

    // Обработчики для кнопок всплывающего окна
    adminPanelButtonEnter.addEventListener('click', function () {
        window.location.href = 'admlogin';
    });

    switchAccountButtonEnter.addEventListener('click', function () {
        window.location.href = 'auth/login';
    });

    stayOnSiteButtonEnter.addEventListener('click', function () {
        popupOverlayLoginButtonEnter.style.display = 'none';
    });
});
