/**
 * Usage: <div use:portal hidden={!isOpen}>...</div>
 * @param {HTMLElement} node 
 */
export function portal(node) {
    let target = document.body;

    function update() {
        target.appendChild(node);
        node.hidden = false;
    }

    function destroy() {
        if (node.parentNode) {
            node.parentNode.removeChild(node);
        }
    }

    update();

    return {
        destroy
    };
}
