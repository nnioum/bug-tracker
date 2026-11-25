import { useState } from 'react'
import { API_URL } from '../api/config.js'
import { AuthContext } from './auth-context.js'

const STORAGE_KEY = 'bugtracker-user'

const roleLabels = {
    TeamLead: 'Тимлид',
    Tester: 'Тестировщик',
    Developer: 'Разработчик',
}

function loadUser() {
    try {
        const saved = localStorage.getItem(STORAGE_KEY)
        return saved ? JSON.parse(saved) : null
    } catch {
        return null
    }
}

function saveUser(user) {
    try {
        if (user) {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(user))
        } else {
            localStorage.removeItem(STORAGE_KEY)
        }
    } catch {
    }
}

export default function AuthProvider({ children }) {
    const [user, setUser] = useState(loadUser)

    async function login(loginValue, password) {
        let response

        try {
            response = await fetch(`${API_URL}/api/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ login: loginValue.trim(), password }),
            })
        } catch {
            return { ok: false, message: 'Не удалось связаться с сервером' }
        }

        if (response.status === 401) {
            return { ok: false, message: 'Неверный логин или пароль' }
        }

        if (!response.ok) {
            return { ok: false, message: 'Ошибка сервера, попробуйте позже' }
        }

        const data = await response.json()
        const nextUser = {
            id: data.userId,
            token: data.token,
            name: data.name,
            role: data.role,
            roleLabel: roleLabels[data.role],
        }

        setUser(nextUser)
        saveUser(nextUser)
        return { ok: true }
    }

    function logout() {
        setUser(null)
        saveUser(null)
    }

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    )
}