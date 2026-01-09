import { useState } from 'react';
import { TaskStatus, getStatusLabel } from '../services/taskApi';
import './TaskItem.css';

function TaskItem({ task, onUpdate, onChangeStatus, onDelete }) {
    const [isEditing, setIsEditing] = useState(false);
    const [isStatusOpen, setIsStatusOpen] = useState(false);
    const [title, setTitle] = useState(task.title);
    const [description, setDescription] = useState(task.description || '');

    const handleSave = async () => {
        if (!title.trim()) return;
        await onUpdate(task.id, { title: title.trim(), description: description.trim() || null });
        setIsEditing(false);
    };

    const handleCancel = () => {
        setTitle(task.title);
        setDescription(task.description || '');
        setIsEditing(false);
    };

    const getStatusColor = (status) => {
        switch (status) {
            case TaskStatus.Pending: return 'status-pending';
            case TaskStatus.InProgress: return 'status-progress';
            case TaskStatus.Completed: return 'status-completed';
            default: return '';
        }
    };

    if (isEditing) {
        return (
            <div className="task-item task-item-editing">
                <div className="task-edit-form">
                    <input
                        type="text"
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        maxLength={200}
                        autoFocus
                    />
                    <textarea
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        placeholder="Description..."
                        maxLength={1000}
                        rows={3}
                    />
                    <div className="task-edit-actions">
                        <button className="btn btn-sm btn-primary" onClick={handleSave}>💾 Save</button>
                        <button className="btn btn-sm btn-secondary" onClick={handleCancel}>✕ Cancel</button>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className={`task-item ${getStatusColor(task.status)} ${isStatusOpen ? 'item-active' : ''}`}>
            <div className="task-content">
                <div className="task-header">
                    <h3 className="task-title">{task.title}</h3>
                    <span className={`task-status-badge ${getStatusColor(task.status)}`}>
                        {getStatusLabel(task.status)}
                    </span>
                </div>
                {task.description && (
                    <p className="task-description">{task.description}</p>
                )}
                <div className="task-meta">
                    <span className="task-date" title={`Created: ${new Date(task.createdAt).toLocaleString()}`}>
                        📅 {new Date(task.createdAt).toLocaleDateString()}
                    </span>
                    {task.updatedAt !== task.createdAt && (
                        <span className="task-date" title={`Last Updated: ${new Date(task.updatedAt).toLocaleString()}`}>
                            🔄 {new Date(task.updatedAt).toLocaleDateString()}
                        </span>
                    )}
                </div>
            </div>
            <div className="task-actions">
                <div className="status-dropdown-container" style={{ position: 'relative' }}>
                    <button
                        className="status-select"
                        onClick={() => setIsStatusOpen(!isStatusOpen)}
                        onBlur={() => setTimeout(() => setIsStatusOpen(false), 200)}
                        title="Change Status"
                    >
                        {task.status === TaskStatus.Pending && '⏳'}
                        {task.status === TaskStatus.InProgress && '🔄'}
                        {task.status === TaskStatus.Completed && '✅'}
                    </button>
                    {isStatusOpen && (
                        <div className="status-menu">
                            <div className="status-option" onClick={() => onChangeStatus(task.id, TaskStatus.Pending)}>
                                ⏳ Pending
                            </div>
                            <div className="status-option" onClick={() => onChangeStatus(task.id, TaskStatus.InProgress)}>
                                🔄 In Progress
                            </div>
                            <div className="status-option" onClick={() => onChangeStatus(task.id, TaskStatus.Completed)}>
                                ✅ Completed
                            </div>
                        </div>
                    )}
                </div>

                <button className="btn-icon" onClick={() => setIsEditing(true)} title="Edit">✏️</button>

                <button
                    className="btn-icon btn-danger"
                    onClick={(e) => {
                        e.stopPropagation();
                        onDelete(task.id);
                    }}
                    title="Delete"
                >
                    🗑️
                </button>
            </div>
        </div>
    );
}

export default TaskItem;
