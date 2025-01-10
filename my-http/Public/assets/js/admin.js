document.addEventListener('DOMContentLoaded', function () {
    // Обработчик для формы добавления пользователя
    document.getElementById('add-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Ошибка при отправке данных');
            }

            const result = await response.json();
            if (result == false) {
                alert('Такой пользователь уже есть');
                throw new Error('Такой пользователь уже есть');
            }

            // Создаем новую строку таблицы
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.Login}</td>
                <td>${result.Password}</td>
            `;
            // Находим таблицу и добавляем новую строку
            const table = document.getElementById('userTableBody');
            table.appendChild(newRow);
            document.getElementById('addUserLogin').value = '';
            document.getElementById('addUserPassword').value = '';
            alert('Вы успешно добавили пользователя!')

        } catch (error) {
            alert('Ошибка при добавлении пользователя: ' + error.message);
        }
    });

    // Обработчик для формы добавления фильма
    document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Ошибка при отправке данных');
            }

            const result = await response.json();
            if (result == false) {
                alert('Такой фильм уже есть');
                throw new Error('Такой фильм уже есть');
            }

            // Создаем новую строку таблицы
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
            <td>${result.MovieId}</td>
            <td>${result.Title}</td>
            <td>${result.ImageUrl}</td>
            <td>${result.HtmlPageUrl}</td>
        `;
            // Находим таблицу и добавляем новую строку
            const table = document.getElementById('movieTableBody');
            table.appendChild(newRow);
            document.getElementById('addTitle').value = '';
            document.getElementById('addImageUrl').value = '';
            document.getElementById('addHtmlPageUrl').value = '';

            alert('Вы успешно добавили фильм!')

        } catch (error) {
            alert('Ошибка при добавлении фильма: ' + error.message);
        }
    });

    // Обработчик для формы добавления MovieData
    document.getElementById('add-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Ошибка при отправке данных');
            }

            const result = await response.json();
            if (result == false) {
                alert('Такой MovieData уже есть');
                throw new Error('Такой MovieData уже есть');
            }

            // Создаем новую строку таблицы
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.MovieId}</td>
                <td>${result.Title}</td>
                <td>${result.CoverImageUrl}</td>
                <td>${result.Description}</td>
                <td>${result.OriginalTitle}</td>
                <td>${result.Year}</td>
                <td>${result.Country}</td>
                <td>${result.Genre}</td>
                <td>${result.Quality}</td>
                <td>${result.Sound}</td>
                <td>${result.Director}</td>
                <td>${result.Cast}</td>
                <td>${result.MoviePlayer}</td>
            `;
            // Находим таблицу и добавляем новую строку
            const table = document.getElementById('movieDataTableBody');
            table.appendChild(newRow);
            document.getElementById('addMovieDataTitle').value = '';
            document.getElementById('addMovieDataCoverImageUrl').value = '';
            document.getElementById('addMovieDataDescription').value = '';
            document.getElementById('addMovieDataOriginalTitle').value = '';
            document.getElementById('addMovieDataYear').value = '';
            document.getElementById('addMovieDataCountry').value = '';
            document.getElementById('addMovieDataGenre').value = '';
            document.getElementById('addMovieDataQuality').value = '';
            document.getElementById('addMovieDataSound').value = '';
            document.getElementById('addMovieDataDirector').value = '';
            document.getElementById('addMovieDataCast').value = '';
            document.getElementById('addMovieDataMoviePlayer').value = '';

            alert('Вы успешно добавили MovieData!')

        } catch (error) {
            alert('Ошибка при добавлении MovieData: ' + error.message);
        }
    });

    // Обработчик для формы добавления Genre
    document.getElementById('add-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Ошибка при отправке данных');
            }

            const result = await response.json();
            if (result == false) {
                alert('Такой Genre уже есть');
                throw new Error('Такой Genre уже есть');
            }

            // Создаем новую строку таблицы
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.GenreName}</td>
            `;
            // Находим таблицу и добавляем новую строку
            const table = document.getElementById('genreTableBody');
            table.appendChild(newRow);
            document.getElementById('addGenreName').value = '';

            alert('Вы успешно добавили Genre!')

        } catch (error) {
            alert('Ошибка при добавлении Genre: ' + error.message);
        }
    });

    // Обработчик для формы добавления Country
    document.getElementById('add-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Ошибка при отправке данных');
            }

            const result = await response.json();
            if (result == false) {
                alert('Такая Country уже есть');
                throw new Error('Такая Country уже есть');
            }

            // Создаем новую строку таблицы
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.CountryName}</td>
            `;
            // Находим таблицу и добавляем новую строку
            const table = document.getElementById('countryTableBody');
            table.appendChild(newRow);
            document.getElementById('addCountryName').value = '';

            alert('Вы успешно добавили Country!')

        } catch (error) {
            alert('Ошибка при добавлении Country: ' + error.message);
        }
    });

    // Обработчик для формы удаления пользователя
    document.getElementById('delete-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();

            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Ошибка при удалении пользователя: ' + result.error);
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('userTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(user => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${user.Id}</td>
                    <td>${user.Login}</td>
                    <td>${user.Password}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Ошибка при удалении пользователя: ' + error.message);
        }
    });

    // Обработчик для формы удаления фильма
    document.getElementById('delete-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();

            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Ошибка при удалении фильма: ' + result.error);
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('movieTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(movie => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${movie.MovieId}</td>
                    <td>${movie.Title}</td>
                    <td>${movie.ImageUrl}</td>
                    <td>${movie.HtmlPageUrl}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Ошибка при удалении фильма: ' + error.message);
        }
    });

    // Обработчик для формы удаления MovieData
    document.getElementById('delete-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();

            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Ошибка при удалении MovieData: ' + result.error);
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('movieDataTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(movieData => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${movieData.MovieId}</td>
                    <td>${movieData.Title}</td>
                    <td>${movieData.CoverImageUrl}</td>
                    <td>${movieData.Description}</td>
                    <td>${movieData.OriginalTitle}</td>
                    <td>${movieData.Year}</td>
                    <td>${movieData.Country}</td>
                    <td>${movieData.Genre}</td>
                    <td>${movieData.Quality}</td>
                    <td>${movieData.Sound}</td>
                    <td>${movieData.Director}</td>
                    <td>${movieData.Cast}</td>
                    <td>${movieData.MoviePlayer}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Ошибка при удалении MovieData: ' + error.message);
        }
    });

    // Обработчик для формы удаления Genre
    document.getElementById('delete-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();

            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Ошибка при удалении Genre: ' + result.error);
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('genreTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(genre => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${genre.Id}</td>
                    <td>${genre.GenreName}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Ошибка при удалении Genre: ' + error.message);
        }
    });

    // Обработчик для формы удаления Country
    document.getElementById('delete-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Предотвращаем стандартную отправку формы

        const form = event.target;

        // Формируем данные из формы
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Выполняем AJAX-запрос
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Ошибка при отправке данных: ' + errorText);
            }

            const result = await response.json();

            // Проверяем, есть ли ошибка в ответе
            if (result.error) {
                alert('Ошибка при удалении Country: ' + result.error);
                return;
            }

            // Очищаем таблицу
            const tableBody = document.getElementById('countryTableBody');
            tableBody.innerHTML = '';

            // Обновляем таблицу новыми данными
            result.forEach(country => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${country.Id}</td>
                    <td>${country.CountryName}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Ошибка при удалении Country: ' + error.message);
        }
    });
});
