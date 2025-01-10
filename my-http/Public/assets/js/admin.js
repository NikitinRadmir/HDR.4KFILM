document.addEventListener('DOMContentLoaded', function () {
    // ���������� ��� ����� ���������� ������������
    document.getElementById('add-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                alert('����� ������������ ��� ����');
                throw new Error('����� ������������ ��� ����');
            }

            // ������� ����� ������ �������
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.Login}</td>
                <td>${result.Password}</td>
            `;
            // ������� ������� � ��������� ����� ������
            const table = document.getElementById('userTableBody');
            table.appendChild(newRow);
            document.getElementById('addUserLogin').value = '';
            document.getElementById('addUserPassword').value = '';
            alert('�� ������� �������� ������������!')

        } catch (error) {
            alert('������ ��� ���������� ������������: ' + error.message);
        }
    });

    // ���������� ��� ����� ���������� ������
    document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                alert('����� ����� ��� ����');
                throw new Error('����� ����� ��� ����');
            }

            // ������� ����� ������ �������
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
            <td>${result.MovieId}</td>
            <td>${result.Title}</td>
            <td>${result.ImageUrl}</td>
            <td>${result.HtmlPageUrl}</td>
        `;
            // ������� ������� � ��������� ����� ������
            const table = document.getElementById('movieTableBody');
            table.appendChild(newRow);
            document.getElementById('addTitle').value = '';
            document.getElementById('addImageUrl').value = '';
            document.getElementById('addHtmlPageUrl').value = '';

            alert('�� ������� �������� �����!')

        } catch (error) {
            alert('������ ��� ���������� ������: ' + error.message);
        }
    });

    // ���������� ��� ����� ���������� MovieData
    document.getElementById('add-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                alert('����� MovieData ��� ����');
                throw new Error('����� MovieData ��� ����');
            }

            // ������� ����� ������ �������
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
            // ������� ������� � ��������� ����� ������
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

            alert('�� ������� �������� MovieData!')

        } catch (error) {
            alert('������ ��� ���������� MovieData: ' + error.message);
        }
    });

    // ���������� ��� ����� ���������� Genre
    document.getElementById('add-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                alert('����� Genre ��� ����');
                throw new Error('����� Genre ��� ����');
            }

            // ������� ����� ������ �������
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.GenreName}</td>
            `;
            // ������� ������� � ��������� ����� ������
            const table = document.getElementById('genreTableBody');
            table.appendChild(newRow);
            document.getElementById('addGenreName').value = '';

            alert('�� ������� �������� Genre!')

        } catch (error) {
            alert('������ ��� ���������� Genre: ' + error.message);
        }
    });

    // ���������� ��� ����� ���������� Country
    document.getElementById('add-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('������ ��� �������� ������');
            }

            const result = await response.json();
            if (result == false) {
                alert('����� Country ��� ����');
                throw new Error('����� Country ��� ����');
            }

            // ������� ����� ������ �������
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.CountryName}</td>
            `;
            // ������� ������� � ��������� ����� ������
            const table = document.getElementById('countryTableBody');
            table.appendChild(newRow);
            document.getElementById('addCountryName').value = '';

            alert('�� ������� �������� Country!')

        } catch (error) {
            alert('������ ��� ���������� Country: ' + error.message);
        }
    });

    // ���������� ��� ����� �������� ������������
    document.getElementById('delete-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();

            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('������ ��� �������� ������������: ' + result.error);
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('userTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
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
            alert('������ ��� �������� ������������: ' + error.message);
        }
    });

    // ���������� ��� ����� �������� ������
    document.getElementById('delete-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();

            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('������ ��� �������� ������: ' + result.error);
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('movieTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
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
            alert('������ ��� �������� ������: ' + error.message);
        }
    });

    // ���������� ��� ����� �������� MovieData
    document.getElementById('delete-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();

            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('������ ��� �������� MovieData: ' + result.error);
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('movieDataTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
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
            alert('������ ��� �������� MovieData: ' + error.message);
        }
    });

    // ���������� ��� ����� �������� Genre
    document.getElementById('delete-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();

            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('������ ��� �������� Genre: ' + result.error);
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('genreTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
            result.forEach(genre => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${genre.Id}</td>
                    <td>${genre.GenreName}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('������ ��� �������� Genre: ' + error.message);
        }
    });

    // ���������� ��� ����� �������� Country
    document.getElementById('delete-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // ������������� ����������� �������� �����

        const form = event.target;

        // ��������� ������ �� �����
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // ��������� AJAX-������
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('������ ��� �������� ������: ' + errorText);
            }

            const result = await response.json();

            // ���������, ���� �� ������ � ������
            if (result.error) {
                alert('������ ��� �������� Country: ' + result.error);
                return;
            }

            // ������� �������
            const tableBody = document.getElementById('countryTableBody');
            tableBody.innerHTML = '';

            // ��������� ������� ������ �������
            result.forEach(country => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${country.Id}</td>
                    <td>${country.CountryName}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('������ ��� �������� Country: ' + error.message);
        }
    });
});
