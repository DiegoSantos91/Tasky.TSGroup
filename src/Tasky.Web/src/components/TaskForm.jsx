import { useState } from 'react';
import './TaskForm.css';

function TaskForm({ onSubmit }) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!title.trim()) return;

        await onSubmit({ title: title.trim(), description: description.trim() || null });
        setTitle('');
        setDescription('');
    };

    return (
        <form className="task-form" onSubmit={handleSubmit}>
            <div className="form-header">
                <h2>✨ Create New Task</h2>
            </div>
            <div className="form-body">
                <div className="form-group">
                    <input
                        type="text"
                        placeholder="Task title..."
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        maxLength={200}
                        required
                    />
                </div>
                <div className="form-group">
                    <textarea
                        placeholder="Description (optional)..."
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        maxLength={1000}
                        rows={3}
                    />
                </div>
                <button type="submit" className="btn btn-primary">
                    <span>➕</span> Add Task
                </button>
            </div>
        </form>
    );
}

export default TaskForm;
