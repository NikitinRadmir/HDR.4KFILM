document.addEventListener('DOMContentLoaded', function () {
    // Handler for the user addition form
    document.getElementById('add-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Error sending data');
            }

            const result = await response.json();
            if (result == false) {
                alert('User already exists');
                throw new Error('User already exists');
            }

            // Create a new table row
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.Login}</td>
                <td>${result.Password}</td>
            `;
            // Find the table and add the new row
            const table = document.getElementById('userTableBody');
            table.appendChild(newRow);
            document.getElementById('addUserLogin').value = '';
            document.getElementById('addUserPassword').value = '';
            alert('You have successfully added a user!')

        } catch (error) {
            alert('Error adding user: ' + error.message);
        }
    });

    // Handler for the movie addition form
    document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Error sending data');
            }

            const result = await response.json();
            if (result == false) {
                alert('Movie already exists');
                throw new Error('Movie already exists');
            }

            // Create a new table row
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
            <td>${result.MovieId}</td>
            <td>${result.Title}</td>
            <td>${result.ImageUrl}</td>
        `;
            // Find the table and add the new row
            const table = document.getElementById('movieTableBody');
            table.appendChild(newRow);
            document.getElementById('addTitle').value = '';
            document.getElementById('addImageUrl').value = '';

            alert('You have successfully added a movie!')

        } catch (error) {
            alert('Error adding movie: ' + error.message);
        }
    });

    // Handler for the MovieData addition form
    document.getElementById('add-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        const addMovieDataId = form.querySelector('#addMovieDataId').value;
        const addMovieDataYear = form.querySelector('#addMovieDataYear').value;

        // Check that MovieId and Year are numbers and positive
        if (isNaN(addMovieDataId) || isNaN(addMovieDataYear) || parseInt(addMovieDataId) <= 0 || parseInt(addMovieDataYear) <= 0) {
            alert('MovieId and Year must be positive numbers.');
            return;
        }

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Error sending data');
            }

            const result = await response.json();
            if (result == false) {
                alert('This MovieData already exists');
                throw new Error('This MovieData already exists');
            }

            // Create a new table row
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
            // Find the table and add the new row
            const table = document.getElementById('movieDataTableBody');
            table.appendChild(newRow);
            document.getElementById('addMovieDataId').value = '';
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

            alert('You have successfully added MovieData!')

        } catch (error) {
            alert('Error adding MovieData: ' + error.message);
        }
    });

    // Handler for the Genre addition form
    document.getElementById('add-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Error sending data');
            }

            const result = await response.json();
            if (result == false) {
                alert('This Genre already exists');
                throw new Error('This Genre already exists');
            }

            // Create a new table row
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.GenreName}</td>
            `;
            // Find the table and add the new row
            const table = document.getElementById('genreTableBody');
            table.appendChild(newRow);
            document.getElementById('addGenreName').value = '';

            alert('You have successfully added a Genre!')

        } catch (error) {
            alert('Error adding Genre: ' + error.message);
        }
    });

    // Handler for the Country addition form
    document.getElementById('add-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                throw new Error('Error sending data');
            }

            const result = await response.json();
            if (result == false) {
                alert('This Country already exists');
                throw new Error('This Country already exists');
            }

            // Create a new table row
            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td>${result.Id}</td>
                <td>${result.CountryName}</td>
            `;
            // Find the table and add the new row
            const table = document.getElementById('countryTableBody');
            table.appendChild(newRow);
            document.getElementById('addCountryName').value = '';

            alert('You have successfully added a Country!')

        } catch (error) {
            alert('Error adding Country: ' + error.message);
        }
    });

    // Handler for the user deletion form
    document.getElementById('delete-user-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            // Check for errors in the response
            if (result.error) {
                alert('Error deleting user: ' + result.error);
                return;
            }

            // Clear the table
            const tableBody = document.getElementById('userTableBody');
            tableBody.innerHTML = '';

            // Update the table with new data
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
            alert('Error deleting user: ' + error.message);
        }
    });

    // Handler for the movie deletion form
    document.getElementById('delete-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            // Check for errors in the response
            if (result.error) {
                alert('Error deleting movie: ' + result.error);
                return;
            }

            // Clear the table
            const tableBody = document.getElementById('movieTableBody');
            tableBody.innerHTML = '';

            // Update the table with new data
            result.forEach(movie => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${movie.MovieId}</td>
                    <td>${movie.Title}</td>
                    <td>${movie.ImageUrl}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Error deleting movie: ' + error.message);
        }
    });

    // Handler for the MovieData deletion form
    document.getElementById('delete-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            // Check for errors in the response
            if (result.error) {
                alert('Error deleting MovieData: ' + result.error);
                return;
            }

            // Clear the table
            const tableBody = document.getElementById('movieDataTableBody');
            tableBody.innerHTML = '';

            // Update the table with new data
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
            alert('Error deleting MovieData: ' + error.message);
        }
    });

    // Handler for the Genre deletion form
    document.getElementById('delete-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            // Check for errors in the response
            if (result.error) {
                alert('Error deleting Genre: ' + result.error);
                return;
            }

            // Clear the table
            const tableBody = document.getElementById('genreTableBody');
            tableBody.innerHTML = '';

            // Update the table with new data
            result.forEach(genre => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${genre.Id}</td>
                    <td>${genre.GenreName}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Error deleting Genre: ' + error.message);
        }
    });

    // Handler for the Country deletion form
    document.getElementById('delete-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            // Check for errors in the response
            if (result.error) {
                alert('Error deleting Country: ' + result.error);
                return;
            }

            // Clear the table
            const tableBody = document.getElementById('countryTableBody');
            tableBody.innerHTML = '';

            // Update the table with new data
            result.forEach(country => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${country.Id}</td>
                    <td>${country.CountryName}</td>
                `;
                tableBody.appendChild(newRow);
            });

        } catch (error) {
            alert('Error deleting Country: ' + error.message);
        }
    });

    // Handler for the User update form
    document.getElementById('update-user-form').addEventListener('submit', async function (event) {
        event.preventDefault();

        const form = event.target;

        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {

            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error: ' + errorText);
            }

            const result = await response.json();
            if (result == false) {
                throw new Error('This user does not exist');
            }

            if (result.error) {
                alert('Error. Incorrect ID');
                return;
            }

            const tableBody = document.getElementById('userTableBody');
            tableBody.innerHTML = '';

            result.forEach(user => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${user.Id}</td>
                    <td>${user.Login}</td>
                    <td>${user.Password}</td>
                `;
                tableBody.appendChild(newRow);

            });
            alert('User updated successfully!')

        } catch (error) {
            alert('Error: ' + error.message);
        }
    });

    document.getElementById('update-movie-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            if (result.error) {
                alert('Error updating Movie: ' + result.error);
                return;
            }

            // Update the table row
            const tableBody = document.getElementById('movieTableBody');
            tableBody.innerHTML = '';

            result.forEach(movie => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${movie.MovieId}</td>
                    <td>${movie.Title}</td>
                    <td>${movie.ImageUrl}</td>
                `;
                tableBody.appendChild(newRow);

            });
            alert('Movie updated successfully!')

        } catch (error) {
            alert('Error updating movie: ' + error.message);
        }
    });

    document.getElementById('update-moviedata-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            if (result.error) {
                alert('Error updating MovieData: ' + result.error);
                return;
            }

            // Update the table row
            

            const tableBody = document.getElementById('movieDataTableBody');
            tableBody.innerHTML = '';

            result.forEach(movieData => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${movieDataMovieId}</td>
                <td>${movieDataTitle}</td>
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
            alert('MovieData updated successfully!')

        } catch (error) {
            alert('Error updating MovieData: ' + error.message);
        }
    });

    document.getElementById('update-genre-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            if (result.error) {
                alert('Error updating Genre: ' + result.error);
                return;
            }

            // Update the table row
            const tableBody = document.getElementById('genreTableBody');
            tableBody.innerHTML = '';

            result.forEach(genre => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${genre.Id}</td>
                <td>${genre.GenreName}</td>
                `;
                tableBody.appendChild(newRow);

            });
            alert('Genre updated successfully!')

        } catch (error) {
            alert('Error updating genre: ' + error.message);
        }
    });

    document.getElementById('update-country-form').addEventListener('submit', async function (event) {
        event.preventDefault(); // Prevent the default form submission

        const form = event.target;

        // Form data
        const formData = new FormData(form);
        const data = new URLSearchParams(formData).toString();

        try {
            // Perform AJAX request
            const response = await fetch(form.action, {
                method: form.method,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: data,
            });

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('Error sending data: ' + errorText);
            }

            const result = await response.json();

            if (result.error) {
                alert('Error updating Country: ' + result.error);
                return;
            }

            // Update the table row
            const tableBody = document.getElementById('countryTableBody');
            tableBody.innerHTML = '';

            result.forEach(country => {
                const newRow = document.createElement('tr');
                newRow.innerHTML = `
                    <td>${country.Id}</td>
                <td>${country.CountryName}</td>
                `;
                tableBody.appendChild(newRow);

            });
            alert('Country updated successfully!')

        } catch (error) {
            alert('Error updating country: ' + error.message);
        }
    });
});
