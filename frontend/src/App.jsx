import { BrowserRouter, Link, Navigate, NavLink, Route, Routes } from 'react-router'
import SoftwarePage from './pages/SoftwarePage.jsx'
import TicketDetailsPage from './pages/TicketDetailsPage.jsx'

export default function App() {
  return (
    <BrowserRouter>
      <div className="app-layout">
        <aside className="sidebar">
          <p className="sidebar-title">Баг-трекер</p>

          <nav className="sidebar-nav" aria-label="Разделы приложения">
            <NavLink to="/software">ПО</NavLink>
            <NavLink to="/tickets">Тикеты</NavLink>
            <NavLink to="/notifications">Уведомления</NavLink>
          </nav>
        </aside>

        <main className="page-content">
          <Routes>
            <Route path="/" element={<Navigate to="/software" replace />} />
            <Route path="/software" element={<SoftwarePage />} />
            <Route path="/tickets" element={
              <>
                <h1>Тикеты</h1>
                <p><Link to="/tickets/12">Открыть тикет BUG-12</Link></p>
              </>
            }
            />
            <Route path="/tickets/:ticketId" element={<TicketDetailsPage />} />
            <Route path="/notifications" element={<h1>Уведомления</h1>} />
            <Route path="*" element={<h1>Страница не найдена</h1>} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  )
}