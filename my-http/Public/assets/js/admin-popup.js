document.addEventListener('DOMContentLoaded', function () {
    const tabs = document.querySelectorAll('.tab');
    const tabContents = document.querySelectorAll('.tab-content');

    tabs.forEach(tab => {
        tab.addEventListener('click', function () {
            const target = this.getAttribute('data-tab');

            tabs.forEach(tab => tab.classList.remove('active'));
            tabContents.forEach(content => content.classList.remove('active'));

            this.classList.add('active');
            document.getElementById(target).classList.add('active');
        });
    });



    function updateTruncateCells() {
        const truncateCells = document.querySelectorAll('.truncate');
        truncateCells.forEach(cell => {
            const fullText = cell.textContent;
            if (fullText.length > 30) {
                cell.textContent = fullText.substring(0, 30) + '...';
                cell.setAttribute('data-full-text', fullText);
            }
        });

        const tableCells = document.querySelectorAll('td');
        const popup = document.getElementById('popup');
        const popupOverlay = document.getElementById('popup-overlay');
        const popupClose = document.getElementById('popup-close');
        const popupContent = document.getElementById('popup-content');

        tableCells.forEach(cell => {
            cell.addEventListener('click', function () {
                const fullText = cell.getAttribute('data-full-text') || cell.textContent;
                popupContent.textContent = fullText;
                popup.style.display = 'block';
                popupOverlay.style.display = 'block';
            });
        });

        popupClose.addEventListener('click', function () {
            popup.style.display = 'none';
            popupOverlay.style.display = 'none';
        });

        popupOverlay.addEventListener('click', function () {
            popup.style.display = 'none';
            popupOverlay.style.display = 'none';
        });
    }

    const returnToSiteButton = document.getElementById('returnToSiteButton');

    if (returnToSiteButton) {
        returnToSiteButton.addEventListener('click', function () {
            window.location.href = 'main';
        });
    }

    updateTruncateCells()
});
