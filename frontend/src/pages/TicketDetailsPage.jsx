import { Link, useParams } from 'react-router'
import { useRef, useState } from 'react'
import { useAuth } from '../auth/useAuth.js'

const demoTicket = {
    id: 12,
    title: 'Не работает кнопка «Подтвердить заказ»',
    description:
        'При нажатии на кнопку «Подтвердить заказ» ничего не происходит, заказ не создаётся. Ожидалось: переход на страницу успешного оформления.',
    steps: [
        'Добавить товар в корзину',
        'Перейти к оформлению заказа',
        'Заполнить адрес доставки',
        'Нажать «Подтвердить заказ»',
    ],
    status: 'New',
    software: 'Интернет-магазин',
    priority: 'Высокий',
    author: 'Тестировщик А.',
    attachments: ['screenshot.png', 'console.log'],
    deadline: '',
    assigneeName: '',
    planningRationale: '',
}

const statusLabels = {
    New: 'Новый',
    Planned: 'Запланирован',
}

export default function TicketDetailsPage() {
    const { ticketId } = useParams()
    const { user } = useAuth()
    const [ticket, setTicket] = useState(demoTicket)
    const [planErrors, setPlanErrors] = useState({})
    const planDialogRef = useRef(null)
    const planFormRef = useRef(null)

    if (ticketId !== String(demoTicket.id)) {
        return (
            <>
                <h1>Тикет не найден</h1>
                <p><Link to="/tickets">К тикетам</Link></p>
            </>
        )
    }

    const canPlan = user?.role === 'TeamLead' && ticket.status === 'New'
    const formattedDeadline = ticket.deadline ? ticket.deadline.split('-').reverse().join('.') : '—'

    const details = [
        ['ПО', ticket.software],
        ['Приоритет', ticket.priority],
        ['Автор', ticket.author],
        ['Дедлайн', formattedDeadline]
        ['Ответственный', ticket.assigneeName || '—'],
        ['Релиз исправления', '—'],
        ['Коммиты / MR', '—'],
    ]

    function openPlanDialog() {
        setPlanErrors({})
        planFormRef.current.reset()
        planDialogRef.current.showModal()
    }

    function closePlanDialog() {
        planDialogRef.current.close()
    }

    function handlePlanSubmit(event) {
        event.preventDefault()

        const fields = new FormData(event.currentTarget)
        const deadline = fields.get('deadline')
        const assigneeName = fields.get('assignee')
        const planningRationale = fields.get('planningRationale').trim()
        const today = new Date().toISOString().slice(0, 10)
        const errors = {}

        if (!deadline) {
            errors.deadline = 'Укажите дедлайн'
        } else if (deadline < today) {
            errors.deadline = 'Дедлайн не может быть раньше сегодняшней даты'
        }

        if (!assigneeName) {
            errors.assignee = 'Выберите ответственного'
        }

        if (!planningRationale) {
            errors.planningRationale = 'Укажите основание планирования'
        }

        setPlanErrors(errors)

        if (Object.keys(errors).length > 0) {
            return
        }

        closePlanDialog()

        planDialogRef.current.close()

        setTicket((previousTicket) => ({
            ...previousTicket,
            deadline,
            assigneeName,
            planningRationale,
            status: 'Planned',
        }))
    }

    return (
        <article className="ticket-card">
            <header className="ticket-card-header">
                <h1>Тикет BUG-{ticket.id}</h1>

                <div className="ticket-card-actions">
                    <button type="button" disabled>Редактировать</button>
                    <button type="button" className="ticket-delete" disabled>Удалить</button>
                </div>
            </header>

            <div className="ticket-card-layout">
                <div>
                    <div className="ticket-card-heading">
                        <h2>{ticket.title}</h2>
                        <span className="ticket-status">{statusLabels[ticket.status]}</span>
                    </div>

                    <section className="ticket-section">
                        <h3>Описание</h3>
                        <p>{ticket.description}</p>
                    </section>

                    <section className="ticket-section">
                        <h3>Шаги воспроизведения</h3>
                        <ol>
                            {ticket.steps.map((step) => <li key={step}>{step}</li>)}
                        </ol>
                    </section>

                    <section className="ticket-section">
                        <h3>Вложения</h3>
                        <ul className="ticket-attachments">
                            {ticket.attachments.map((name) => (
                                <li key={name}>
                                    <div className="ticket-attachment-preview" aria-hidden="true" />
                                    <span>{name}</span>
                                </li>
                            ))}
                        </ul>
                    </section>
                </div>

                <aside className="ticket-summary">
                    <dl className="ticket-details">
                        {details.map(([label, value]) => (
                            <div key={label}>
                                <dt>{label}</dt>
                                <dd>{value}</dd>
                            </div>
                        ))}
                    </dl>

                    {canPlan && (
                        <div className="ticket-summary-actions">
                            <button type="button" className="plan-button" onClick={openPlanDialog}>
                                Запланировать исправление
                            </button>
                        </div>
                    )}
                </aside>
            </div>

            <dialog
                ref={planDialogRef}
                className="plan-dialog"
                aria-labelledby="plan-dialog-title"
            >
                <form ref={planFormRef} onSubmit={handlePlanSubmit} noValidate>
                    <h2 id="plan-dialog-title">Запланировать исправление</h2>

                    <div className="plan-field">
                        <label htmlFor="plan-deadline">Дедлайн</label>
                        <input
                            id="plan-deadline"
                            name="deadline"
                            type="date"
                            required
                            aria-invalid={Boolean(planErrors.deadline)}
                            aria-describedby={planErrors.deadline ? 'plan-deadline-error' : undefined}
                        />
                        {planErrors.deadline && (
                            <p id="plan-deadline-error" className="plan-error">
                                {planErrors.deadline}
                            </p>
                        )}
                    </div>

                    <div className="plan-field">
                        <label htmlFor="plan-assignee">Ответственный</label>
                        <select
                            id="plan-assignee"
                            name="assignee"
                            defaultValue=""
                            required
                            aria-invalid={Boolean(planErrors.assignee)}
                            aria-describedby={planErrors.assignee ? 'plan-assignee-error' : undefined}
                        >
                            <option value="" disabled>Выберите разработчика</option>
                            <option value="Иванов И.">Иванов И.</option>
                        </select>
                        {planErrors.assignee && (
                            <p id="plan-assignee-error" className="plan-error">
                                {planErrors.assignee}
                            </p>
                        )}
                    </div>

                    <div className="plan-field">
                        <label htmlFor="plan-rationale">Основание / комментарий</label>
                        <textarea
                            id="plan-rationale"
                            name="planningRationale"
                            placeholder="Почему и в каком порядке исправлять"
                            rows={3}
                            maxLength={2000}
                            required
                            aria-invalid={Boolean(planErrors.planningRationale)}
                            aria-describedby={planErrors.planningRationale ? 'plan-rationale-error' : undefined}
                        />
                        {planErrors.planningRationale && (
                            <p id="plan-rationale-error" className="plan-error">
                                {planErrors.planningRationale}
                            </p>
                        )}
                    </div>

                    <div className="plan-dialog-actions">
                        <button
                            type="button"
                            className="plan-button"
                            onClick={() => planDialogRef.current.close()}
                        >
                            Отмена
                        </button>
                        <button type="submit" className="plan-button plan-button-primary">
                            Сохранить
                        </button>
                    </div>
                </form>
            </dialog>
        </article>
    )
}