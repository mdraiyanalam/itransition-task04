import { useState, useEffect } from 'react';
import axios from 'axios';

interface User {
    id: number;
    name: string;
    email: string;
    status: string;
    lastLoginTime?: string;
}

const Dashboard = ({ onLogout }: { onLogout: () => void }) => {
    const [users, setUsers] = useState<User[]>([]);
    const [selectedIds, setSelectedIds] = useState<number[]>([]);
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState(false);
    const [message, setMessage] = useState<{ text: string; type: string }>({ text: '', type: '' });

    const token = localStorage.getItem('token');

    const fetchUsers = async () => {
        try {
            setLoading(true);
            const response = await axios.get('https://itransition-task04-backendwebservices.onrender.com/api/users', {
                headers: { Authorization: `Bearer ${token}` }
            });
            setUsers(response.data);
        } catch (err: any) {
            setMessage({ text: 'Failed to load users', type: 'danger' });
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, []);

    const toggleSelect = (id: number) => {
        setSelectedIds(prev => prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]);
    };

    const selectAll = () => {
        setSelectedIds(selectedIds.length === users.length ? [] : users.map(u => u.id));
    };

    const performAction = async (action: string) => {
        if (selectedIds.length === 0) return;

        setActionLoading(true);
        try {
            let url = '';
            if (action === 'block') url = '/api/users/block';
            else if (action === 'unblock') url = '/api/users/unblock';
            else if (action === 'delete') url = '/api/users/delete';
            else if (action === 'delete-unverified') url = '/api/users/delete-unverified';

            await axios.post(`https://itransition-task04-backendwebservices.onrender.com${url}`, selectedIds, {
                headers: { Authorization: `Bearer ${token}` }
            });

            setMessage({ text: `Users ${action}ed successfully`, type: 'success' });
            setSelectedIds([]);
            fetchUsers();
        } catch (err: any) {
            setMessage({ text: 'Action failed: ' + (err.response?.data || err.message), type: 'danger' });
        } finally {
            setActionLoading(false);
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>User Management</h2>
                <button className="btn btn-danger" onClick={onLogout}>Logout</button>
            </div>

            {message.text && (
                <div className={`alert alert-${message.type}`}>{message.text}</div>
            )}

            {/* Toolbar */}
            <div className="mb-3">
                <button className="btn btn-warning me-2" onClick={() => performAction('block')} disabled={actionLoading}>
                    Block
                </button>
                <button className="btn btn-success me-2" onClick={() => performAction('unblock')} disabled={actionLoading}>
                    Unblock
                </button>
                <button className="btn btn-danger me-2" onClick={() => performAction('delete')} disabled={actionLoading}>
                    Delete
                </button>
                <button className="btn btn-secondary" onClick={() => performAction('delete-unverified')} disabled={actionLoading}>
                    Delete Unverified
                </button>
            </div>

            {/* Table */}
            <table className="table table-striped table-hover">
                <thead className="table-dark">
                    <tr>
                        <th>
                            <input type="checkbox" checked={selectedIds.length === users.length && users.length > 0} onChange={selectAll} />
                        </th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Status</th>
                        <th>Last Login</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user => (
                        <tr key={user.id}>
                            <td>
                                <input type="checkbox" checked={selectedIds.includes(user.id)} onChange={() => toggleSelect(user.id)} />
                            </td>
                            <td>{user.name}</td>
                            <td>{user.email}</td>
                            <td>
                                <span className={`badge ${user.status === 'Active' ? 'bg-success' : user.status === 'Blocked' ? 'bg-danger' : 'bg-warning'}`}>
                                    {user.status}
                                </span>
                            </td>
                            <td>{user.lastLoginTime ? new Date(user.lastLoginTime).toLocaleString() : 'Never'}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {loading && <p className="text-center">Loading users...</p>}
        </div>
    );
};

export default Dashboard;