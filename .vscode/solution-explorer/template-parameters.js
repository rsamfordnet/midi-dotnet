const path = require("path");

// https://thewebdev.info/2021/07/31/how-to-insert-a-space-before-each-capital-letter-with-javascript/
const spaceify = (str) => str.replace(/([A-Z])/g, ' $1').trim();
// https://stackoverflow.com/a/67243723/483623
const kebabize = (str) => str.replace(/[A-Z]+(?![a-z])|[A-Z]/g, ($, ofs) => (ofs ? "-" : "") + $.toLowerCase())

module.exports = function(filename, projectPath, folderPath, xml) {
    let namespace;
    xml.elements.some(e => {
        if (e.name === 'PropertyGroup') {
            const rootNamespace = e.elements.find(p => p.name === 'RootNamespace');
            if (rootNamespace && rootNamespace.elements && rootNamespace.elements[0]) {
                namespace = rootNamespace.elements[0].text;
                return true;
            }
        }
    });

    if (!namespace && projectPath) {
        namespace = path.basename(projectPath, path.extname(projectPath));
        if (folderPath) {
            namespace += "." + folderPath.replace(path.dirname(projectPath), "").substring(1).replace(/[\\\/]/g, ".");
        }
        namespace = namespace.replace(/[\\\-]/g, "_");
    }

    if (!namespace) {
        namespace = "Unknown";
    }

	const name = path.basename(filename, path.extname(filename));

    return {
        namespace: namespace,
        name: name,
		url: kebabize(namespace).replace('.-', '/'),
		title: spaceify(name)
    }
};
