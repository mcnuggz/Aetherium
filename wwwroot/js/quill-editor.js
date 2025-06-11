const QuillEditor = {
    props: {
        name: { type: String, required: true },
        placeholder: { type: String, default: "Write something here..." },
        initialContent: { type: String, default: "" },
        height: { type: String, default: "100px" }
    },
    template: `
        <div>
            <div ref="editorRef" :style="{ height: height }"></div>
            <input type="hidden" :name="name" :value="content" />
        </div>
    `,
    setup(props) {
        const editorRef = Vue.ref(null);
        const content = Vue.ref(props.initialContent);

        Vue.onMounted(() => {
            const quill = new Quill(editorRef.value, {
                theme: 'snow',
                placeholder: props.placeholder,
                modules: {
                    toolbar: {
                        container: [
                            ['bold', 'italic', 'underline'],
                            ['link', 'image']
                        ],
                        handlers: {
                            image: function () {
                                const input = document.createElement('input');
                                input.setAttribute('type', 'file');
                                input.setAttribute('accept', 'image/*');
                                input.click();

                                input.onchange = async () => {
                                    const file = input.files[0];
                                    if (!file) return;

                                    const formData = new FormData();
                                    formData.append('image', file);

                                    try {
                                        const response = await fetch('/api/upload/image', {
                                            method: 'POST',
                                            body: formData
                                        });

                                        const result = await response.json();
                                        if (result.imageUrl) {
                                            const range = quill.getSelection(true);
                                            quill.insertEmbed(range.index, 'image', result.imageUrl);
                                            quill.setSelection(range.index + 1);
                                        } else {
                                            console.error("Upload failed: no imageUrl in response");
                                        }
                                    }
                                    catch (error) {
                                        console.error("Image upload failed: ", error);
                                    }
                                };
                            }
                        }
                    }
                }
            });

            quill.root.innerHTML = content.value;
            const quillEditorElement = editorRef.value.querySelector('.ql-editor');

            quill.on('text-change', () => {
                content.value = quill.root.innerHTML;
                if (content.value.includes("<img")) {
                    quillEditorElement.style.height = "auto";
                    quillEditorElement.style.minHeight = props.height;
                } else {
                    quillEditorElement.style.height = props.height;
                }
            });

            editorRef.value.addEventListener('click', () => quill.focus());
            });

        return { editorRef, content };
    }
};

window.QuillEditor = QuillEditor;