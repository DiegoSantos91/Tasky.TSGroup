import { useState, useEffect } from 'react';
import './App.css';
import { taskApi, TaskStatus, getStatusLabel } from './services/taskApi';
import TaskForm from './components/TaskForm';
import TaskList from './components/TaskList';
import TaskFilters from './components/TaskFilters';
import ConfirmationModal from './components/ConfirmationModal';

function App() {
  const [tasks, setTasks] = useState([]);
  const [filteredTasks, setFilteredTasks] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [statusFilter, setStatusFilter] = useState('all');
  const [deleteModal, setDeleteModal] = useState({ isOpen: false, taskId: null });

  useEffect(() => {
    loadTasks();
  }, []);

  useEffect(() => {
    filterTasks();
  }, [tasks, statusFilter]);

  const loadTasks = async () => {
    try {
      setIsLoading(true);
      const data = await taskApi.getAllTasks();
      setTasks(data);
      setError(null);
    } catch (err) {
      setError('Failed to load tasks. Please try again.');
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  const filterTasks = () => {
    if (statusFilter === 'all') {
      setFilteredTasks(tasks);
    } else {
      setFilteredTasks(tasks.filter(task => task.status === parseInt(statusFilter)));
    }
  };

  const handleCreateTask = async (taskData) => {
    try {
      await taskApi.createTask(taskData);
      await loadTasks();
    } catch (err) {
      setError('Failed to create task');
      console.error(err);
    }
  };

  const handleUpdateTask = async (id, taskData) => {
    try {
      await taskApi.updateTask(id, taskData);
      await loadTasks();
    } catch (err) {
      setError('Failed to update task');
      console.error(err);
    }
  };

  const handleChangeStatus = async (id, status) => {
    try {
      await taskApi.changeTaskStatus(id, status);
      await loadTasks();
    } catch (err) {
      setError('Failed to change task status');
      console.error(err);
    }
  };

  const handleDeleteTask = async (id) => {
    try {
      await taskApi.deleteTask(id);
      await loadTasks();
    } catch (err) {
      setError('Failed to delete task');
      console.error(err);
    }
  };

  const requestDelete = (id) => {
    setDeleteModal({ isOpen: true, taskId: id });
  };

  const confirmDelete = async () => {
    if (deleteModal.taskId) {
      await handleDeleteTask(deleteModal.taskId);
      setDeleteModal({ isOpen: false, taskId: null });
    }
  };

  return (
    <div className="app">
      <header className="app-header">
        <h1>📝 Tasky</h1>
        <p>The Task Management App</p>
      </header>

      <main className="app-main">
        <div className="container">
          {error && (
            <div className="error-banner">
              {error}
              <button onClick={() => setError(null)}>✕</button>
            </div>
          )}

          <TaskForm onSubmit={handleCreateTask} />

          <TaskFilters
            currentFilter={statusFilter}
            onFilterChange={setStatusFilter}
            taskCounts={{
              all: tasks.length,
              pending: tasks.filter(t => t.status === TaskStatus.Pending).length,
              inProgress: tasks.filter(t => t.status === TaskStatus.InProgress).length,
              completed: tasks.filter(t => t.status === TaskStatus.Completed).length
            }}
          />

          {isLoading ? (
            <div className="loading">Loading tasks...</div>
          ) : (
            <TaskList
              tasks={filteredTasks}
              onUpdateTask={handleUpdateTask}
              onChangeStatus={handleChangeStatus}
              onDeleteTask={requestDelete}
            />
          )}

          <ConfirmationModal
            isOpen={deleteModal.isOpen}
            title="Delete Task?"
            message="Are you sure you want to delete this task? This action cannot be undone."
            onConfirm={confirmDelete}
            onCancel={() => setDeleteModal({ isOpen: false, taskId: null })}
          />
        </div>
      </main>

      <footer className="app-footer">
        <p>Built by Diego Santos</p>
      </footer>
    </div>
  );
}

export default App;
