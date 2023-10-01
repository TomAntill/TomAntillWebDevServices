//var settings = {
//    anchorId: null,
//    filters: [],
//    title: "",
//    getImagesUrl: "",
//};

function init(settings) {
    //settings = options;
    createContainer(settings);
    setFilters(settings);
    loadImages(settings);
}

const classes = {
    $glightbx: "glightbox",
    $isotope: "portfolio-isotope",
    $portFilters: 'portfolio-flters',
    $fltActive: 'filter-active',
    $portItem: 'portfolio-item',
    $portContainer: 'portfolio-container',
    $navTabs: 'nav-tabs-wrapper'
}

const selectors = {
    $tabs: "#tabs-",
    $imgContainer: "#imageContainer-",
    $glightbx: `.${classes.$glightbx}`,
    $isotope: `.${classes.$isotope}`,
    $portFilters: `.${classes.$portFilters}`,
    $fltActive: `.${classes.$fltActive}`,
    $portItem: `.${classes.$portItem}`,
    $portContainer: `.${classes.$portContainer}`,
    $navTabs: `.${classes.$navTabs}`,
    $dataPortFilter: 'data-portfolio-filter',
    $dataPortLayout: 'data-portfolio-layout',
    $dataPortSort: 'data-portfolio-sort',
    $dataFilter: 'data-filter',
    $galleryMain: '#gallery-main-',
    $loading: '#loading'
}

function createContainer(settings) {
    if (settings.anchorId == null) {
        throw new Error("gallery anchor id not set");
    }
    // get the anchor id and init base container
    $(`#${settings.anchorId}`).append(
        `<div class='container aos-init aos-animate' data-aos='fade-up'>
        <div class="${classes.$isotope}-${settings.anchorId}" data-portfolio-filter="" data-portfolio-layout="" data-portfolio-sort="original-order">
            <h2 class="text-center headingSpacing" style="color:#d1b181 !important">${settings.title}</h2>
            <div id="gallery-main-${settings.anchorId}" class="loadWrapper">
                <div class="nav-tabs-navigation">
                    <div class="${classes.$navTabs}-${settings.anchorId} hide">
                        <ul id="tabs-${settings.anchorId}" role="tablist" class="${classes.$portFilters} nav nav-tabs aos-init aos-animate" data-aos="fade-up" data-aos-delay="100">
                        </ul>
                    <div>
                </div>
            </div>
        </div>
        <div class="row gy-4 ${classes.$portContainer}-${settings.anchorId} hide" data-aos="fade-up" data-aos-delay="200" id="imageContainer-${settings.anchorId}">
        </div>
    </div>`
    );
}

function setFilters(settings) {
    if (settings.filters.length > 0) {
        var tbs = $(`${selectors.$tabs}${settings.anchorId}`);
        let isDefault = false;
        settings.filters.forEach(f => {
            const div = document.createElement("div");
            if (f.filter === settings.defaultFilter) {
                isDefault = true;
            }
            const fitlerHtml = `<li data-filter=${f.filter} class="${isDefault ? `filter-active subheading-spacing` : ``} nav-item">
                                    <a class="nav-link ${isDefault ? `active` : ``}  subheading-spacing" data-toggle="tab" href="${f.href}" role="tab" aria-selected="true">${f.display}</a>
                                </li>`;
            div.innerHTML = fitlerHtml;
            tbs.append(div.firstChild);
            if (isDefault) {
                isDefault = false;
            }
        });
    }
}

function loadImages(settings) {
    loading(settings);
    return new Promise((resolve, reject) => {
        const apiUrl = new URL(settings.getImagesUrl);
        const xhr = new XMLHttpRequest();
        xhr.open("GET", apiUrl.toString());
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    const response = JSON.parse(xhr.responseText);
                    resolve(response);
                } else {
                    reject(new Error(`Request failed with status: ${xhr.status}`));
                }
            }
        };
        xhr.send();
    }).then(images => {
        renderImages(images, settings).then(_ => {
            loadComplete(settings);
            initGallery(settings);
            initSwiper(settings);
            initGBox(settings);
        });
    }).catch(error => {
        console.error(error);
    });;
}

function renderImages(images, settings) {
    return new Promise((resolve, reject) => {
        var imageContainer = $(`${selectors.$imgContainer}${settings.anchorId}`);
        var imagePromises = [];
        images.forEach(image => {

            const portfolioItem = document.createElement('div');
            portfolioItem.className = `col-lg-4 col-md-6 portfolio-item filter-${image.pictureCategory}`;
            portfolioItem.classList.add(`${classes.$portItem}`);
            const portfolioContent = document.createElement('div');
            portfolioContent.className = 'portfolio-content h-100';
            const imgElement = document.createElement('img');
            imgElement.src = image.url;
            imgElement.className = 'img-fluid';
            const portfolioInfo = document.createElement('div');
            portfolioInfo.className = 'portfolio-info';
            const previewLink = document.createElement('a');
            previewLink.href = image.url;
            previewLink.title = image.pictureCategory;
            previewLink.className = 'glightbox preview-link';
            previewLink.setAttribute('data-gallery', `portfolio-gallery-${image.pictureCategory}`);
            const zoomIcon = document.createElement('i');
            zoomIcon.className = 'bi bi-zoom-in';
            previewLink.appendChild(zoomIcon);
            portfolioInfo.appendChild(previewLink);
            portfolioContent.appendChild(imgElement);
            portfolioContent.appendChild(portfolioInfo);
            portfolioItem.appendChild(portfolioContent);

            imagePromises.push(new Promise((resolve, reject) => {
                imgElement.onload = () => resolve();
                imgElement.onerror = () => reject();
            }));

            imageContainer.append(portfolioItem);
        });

        Promise.all(imagePromises)
            .then(() => resolve())
            .catch(() => reject());
    });
}

function loading(settings) {
    const styleElement = document.createElement("style");
    const cssStyles = `
    #loading-${settings.anchorId} {       
        width: 50px;
        height: 50px;
        border: 1px solid #a9b4c2;
        border-radius: 50%;
        border-top-color: #fff;
        animation: spin 1s ease-in-out infinite;
        -webkit-animation: spin 1s ease-in-out infinite;
    }
    @keyframes spin {
        to { -webkit-transform: rotate(360deg); }
    }
    @-webkit-keyframes spin {
        to { -webkit-transform: rotate(360deg); }
    }
    .loadWrapper {
      display: flex;
      justify-content: center;
      flex-direction: row;
      padding: 2rem;
    }`;
    styleElement.textContent = cssStyles;
    document.head.appendChild(styleElement);
    var galleryMain = $(`${selectors.$galleryMain}${settings.anchorId}`);
    galleryMain.append(`<div id="loading-${settings.anchorId}"></div>`);
}

function loadComplete(settings) {
    var galleryMain = $(`${selectors.$galleryMain}${settings.anchorId}`);
    var container = $(`${selectors.$imgContainer}${settings.anchorId}`);
    var tabs = $(`${selectors.$navTabs}-${settings.anchorId}`);
    var loading = $(`${selectors.$loading}-${settings.anchorId}`);
    galleryMain.removeClass('loadWrapper');
    container.removeClass('hide');
    tabs.removeClass('hide');
    loading.addClass('hide');
}

function initGBox(settings) {
    const glightbox = GLightbox({
        selector: selectors.$glightbx
    });
}

function initGallery(settings) {
    let portfolionIsotope = document.querySelector(`${selectors.$isotope}-${settings.anchorId}`);
    if (portfolionIsotope) {

        let portfolioFilter = portfolionIsotope.getAttribute(selectors.$dataPortFilter) ? portfolionIsotope.getAttribute(selectors.$dataPortFilter) : '*';
        let portfolioLayout = portfolionIsotope.getAttribute(selectors.$dataPortLayout) ? portfolionIsotope.getAttribute(selectors.$dataPortLayout) : 'masonry';
        let portfolioSort = portfolionIsotope.getAttribute(selectors.$dataPortSort) ? portfolionIsotope.getAttribute(selectors.$dataPortSort) : 'original-order';

        let portfolioIsotope = new Isotope(document.querySelector(`${selectors.$portContainer}-${settings.anchorId}`), {
            itemSelector: selectors.$portItem,
            layoutMode: portfolioLayout,
            filter: portfolioFilter,
            sortBy: portfolioSort
        });
        let menuFilters = document.querySelectorAll(`${selectors.$isotope}-${settings.anchorId}` + ' ' + selectors.$portFilters + ' ' + 'li');
        menuFilters.forEach(function (el) {
            el.addEventListener('click', function () {
                document.querySelector(`${selectors.$isotope}-${settings.anchorId}` + ' ' + selectors.$portFilters + ' ' + selectors.$fltActive).classList.remove(selectors.$fltActive);
                this.classList.add(selectors.$fltActive);
                portfolioIsotope.arrange({
                    filter: this.getAttribute(selectors.$dataFilter)
                });
                if (typeof aos_init === 'function') {
                    aos_init();
                }
            }, false);
        });
        portfolioIsotope.arrange({
            filter: settings.defaultFilter
        });
    }
}

function initSwiper(settings) {
    new Swiper('.slides-1', {
        speed: 600,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        slidesPerView: 'auto',
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        }
    });

    new Swiper('.slides-2', {
        speed: 600,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        slidesPerView: 'auto',
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        },
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        breakpoints: {
            320: {
                slidesPerView: 1,
                spaceBetween: 20
            },

            1200: {
                slidesPerView: 2,
                spaceBetween: 20
            }
        }
    });
}

const dynamicGallery = {
    init: init
}