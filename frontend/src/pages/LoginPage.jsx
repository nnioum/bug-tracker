import { useState } from 'react'
import { useAuth } from '../auth/useAuth.js'

export default function LoginPage() {
    const { login } = useAuth()
    const [loginValue, setLoginValue] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')

    function handleSubmit(event) {
        event.preventDefault()

        if (!loginValue.trim() || !password) {
            setError('Введите логин и пароль')
            return
        }

        if (!login(loginValue)) {
            setError('Неверный логин или пароль')
        }
    }

    const inputClass = error ? 'login-input login-input-error' : 'login-input'

    return (
        <div className="login-page">
            <form className="login-card" onSubmit={handleSubmit}>
                <h1 className="login-title">Баг-трекер</h1>
                <p className="login-subtitle">Войдите, чтобы продолжить</p>

                <label className="login-field">
                    Логин
                    <input
                        className={inputClass}
                        type="text"
                        value={loginValue}
                        onChange={(event) => setLoginValue(event.target.value)}
                    />
                </label>

                <label className="login-field">
                    Пароль
                    <input
                        className={inputClass}
                        type="password"
                        value={password}
                        onChange={(event) => setPassword(event.target.value)}
                    />
                </label>

                {error && <p className="login-error">{error}</p>}

                <button className="login-button" type="submit">Войти</button>
            </form>
        </div>
    )
}