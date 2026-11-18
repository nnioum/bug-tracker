import { useState } from 'react'
import { AuthContext } from './auth-context.js'

const STORAGE_KEY = 'bugtracker-user'

const demoUsers = {
    lead: { name: 'Тимлид', role: 'TeamLead', roleLabel: 'Тимлид' },
    tester: { name: 'Тестировщик А.', role: 'Tester', roleLabel: 'Тестировщик' },
    dev: { name: 'Иванов И.', role: 'Developer', roleLabel: 'Разработчик' },
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

    function login(loginValue) {
        const found = demoUsers[loginValue.trim().toLowerCase()]
        if (!found) {
            return false
        }
        setUser(found)
        saveUser(found)
        return true
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