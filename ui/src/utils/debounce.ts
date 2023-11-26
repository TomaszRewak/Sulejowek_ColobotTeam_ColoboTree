export const createDebouncedFunction = (callback: Function, delay: number) => {
    let timeoutId: any;

    return function () {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => {
            callback.apply(this, arguments);
        }, delay);
    };
}