/**
 * Builds the DOM for the global navigation optionally specifying which menu item is active.
 *
 * @param {string} url - The menu item which is active.
 */
const buildGlobalNavbar = selectedMenuItem => {
    selectedMenuItem = selectedMenuItem || '';

    const nav = document.createElement('nav');
    nav.classList.add('navbar', 'navbar-expand-lg', 'bg-body-tertiary', 'mb-5');

    const container = document.createElement('div');
    container.classList.add('container-fluid');
    nav.appendChild(container);

    const buttonToggle = document.createElement('button');
    buttonToggle.classList.add('navbar-toggler');
    buttonToggle.setAttribute('type', 'button');
    buttonToggle.dataset.bsToggle = 'collapse';
    buttonToggle.dataset.bsTarget = '#navbar-global';
    buttonToggle.ariaControls = 'navbar-global';
    buttonToggle.ariaExpanded = 'false';
    buttonToggle.ariaLabel = 'Toggle navigation';
    container.appendChild(buttonToggle);

    const navbarToggleIcon = document.createElement('span');
    navbarToggleIcon.classList.add('navbar-toggler-icon');
    buttonToggle.appendChild(navbarToggleIcon);

    const navbar = document.createElement('div');
    navbar.id = 'navbar-global';
    navbar.classList.add('collapse', 'navbar-collapse');
    container.appendChild(navbar);

    const navbarNav = document.createElement('ul');
    navbarNav.classList.add('navbar-nav', 'mx-lg-auto');
    navbar.appendChild(navbarNav);

    const navbarItemInventory = document.createElement('li');
    navbarItemInventory.classList.add('nav-item');
    navbarNav.appendChild(navbarItemInventory);

    const anchorInventory = document.createElement('a');
    anchorInventory.setAttribute('href', 'http://web-pages.local/Inventory/Index.html');
    anchorInventory.innerText = 'Inventory';
    anchorInventory.classList.add('nav-link');
    if (selectedMenuItem.toLowerCase() === 'inventory') {
        anchorInventory.classList.add('active');
        anchorInventory.ariaCurrent = 'page';
    }
    navbarItemInventory.appendChild(anchorInventory);

    const navbarItemWidgets = document.createElement('li');
    navbarItemWidgets.classList.add('nav-item');
    navbarNav.appendChild(navbarItemWidgets);

    const anchorWidgets = document.createElement('a');
    anchorWidgets.setAttribute('href', 'http://web-pages.local/Widgets/Index.html');
    anchorWidgets.innerText = 'Widgets';
    anchorWidgets.classList.add('nav-link');
    if (selectedMenuItem.toLowerCase() === 'widgets') {
        anchorWidgets.classList.add('active');
        anchorWidgets.ariaCurrent = 'page';
    }
    navbarItemWidgets.appendChild(anchorWidgets);

    const navbarItemWidgetTypes = document.createElement('li');
    navbarItemWidgetTypes.classList.add('nav-item');
    navbarNav.appendChild(navbarItemWidgetTypes);

    const anchorWidgetTypes = document.createElement('a');
    anchorWidgetTypes.setAttribute('href', 'http://web-pages.local/WidgetTypes/Index.html');
    anchorWidgetTypes.innerText = 'Widget Types';
    anchorWidgetTypes.classList.add('nav-link');
    if (selectedMenuItem.toLowerCase() === 'widget-types') {
        anchorWidgetTypes.classList.add('active');
        anchorWidgetTypes.ariaCurrent = 'page';
    }
    navbarItemWidgetTypes.appendChild(anchorWidgetTypes);

    return nav;
};

/**
 * Builds a web request object with specified URL, method, and body.
 *
 * @param {string} url - The endpoint URL.
 * @param {string} method - The HTTP method (GET, POST, etc.).
 * @param {Object} body - The request payload.
 * @returns {Object} The constructed web request object.
 */
const buildWebRequest = (url, method, body) => {
    return {
        Body: JSON.stringify(body || {}),
        Method: method || 'GET',
        Origin: window.location.pathname.substring(window.location.pathname.indexOf('WebPages/')),
        Url: url || ''
    };
};

/**
 * Dispatches a custom 'messageReceived' event with the provided request and response details.
 *
 * @param {Object} request - The web request details.
 * @param {Object} response - The web response details.
 */
const dispatchMessageReceivedEvent = (request, response) => {
    const messageReceivedEvent = new CustomEvent('messageReceived', {
        detail: { request, response },
        bubbles: true,
        cancelable: true
    });
    window.dispatchEvent(messageReceivedEvent);
};

/**
 * Converts a string into a slug.
 *
 * @param {string} str - The string to be slugified.
 * @returns {string} The slugified string.
 */
const slugify = str => {
    return String(str)
        .normalize('NFKD')
        .replace(/[\u0300-\u036f]/g, '')
        .trim()
        .toLowerCase()
        .replace(/[^a-z0-9 -]/g, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-');
}

/**
 * Converts a given value to a currency format string based on the specified locale and currency.
 *
 * @param {number|string} value - The value to be formatted. Can be a number or a string that represents a number.
 * @param {string} [locale='en-US'] - The locale string (e.g., 'en-US', 'de-DE') for formatting. Defaults to 'en-US' if not provided.
 * @param {string} [currency='USD'] - The ISO 4217 currency code (e.g., 'USD', 'EUR') for formatting. Defaults to 'USD' if not provided.
 * @returns {string} - The formatted currency string.
 * @throws {Error} - Throws an error if the provided value cannot be converted to a number.
 *
 * @example
 * // returns "$1,000.00"
 * toCurrencyFormat(1000);
 *
 * @example
 * // returns "€1.000,00"
 * toCurrencyFormat(1000, 'de-DE', 'EUR');
 *
 * @example
 * // returns "¥1,000"
 * toCurrencyFormat('1000', 'ja-JP', 'JPY');
 */
const toCurrencyFormat = (value, locale, currency) => {
    locale = locale || 'en-US';
    currency = currency || 'USD';

    const number = parseFloat(value);

    // Check if the conversion was successful
    if (isNaN(number)) {
        throw new Error('Invalid number string');
    }

    // Format the number as currency
    const formatter = new Intl.NumberFormat(locale, {
        style: 'currency',
        currency: currency,
    });

    const currencyString = formatter.format(number);
    return currencyString;
};

/**
 * Toggles the visibility of a loading overlay.
 *
 * @param {boolean} isVisible - Indicates whether the loader should be visible.
 */
const toggleLoader = isVisible => {
    let overlay = document.querySelector('.overlay-loading');
    if (isVisible) {
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.classList.add('position-fixed', 'top-0', 'start-0', 'vw-100', 'vh-100', 'd-flex', 'justify-content-center', 'align-items-center', 'overlay-loading', 'opacity-50');
            overlay.style.zIndex = '9999';

            const image = document.createElement('div');
            image.classList.add('spinner-border');
            overlay.appendChild(image);
            document.body.appendChild(overlay);
        }
        if (overlay.classList.contains('d-none')) {
            overlay.classList.remove('d-none');
        }
    } else if (overlay && !overlay.classList.contains('d-none')) {
        overlay.classList.add('d-none');
    }
};
