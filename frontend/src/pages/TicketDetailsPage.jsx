import { Link, useParams } from 'react-router'

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
    status: 'Новый',
    software: 'Интернет-магазин',
    priority: 'Высокий',
    author: 'Тестировщик А.',
    attachments: ['screenshot.png', 'console.log'],
}

export default function TicketDetailsPage() {
    const { ticketId } = useParams()

    if (ticketId !== String(demoTicket.id)) {
        return (
            <>
                <h1>Тикет не найден</h1>
                <p><Link to="/tickets">К тикетам</Link></p>
            </>
        )
    }

    const ticket = demoTicket
    const details = [
        ['ПО', ticket.software],
        ['Приоритет', ticket.priority],
        ['Автор', ticket.author],
        ['Дедлайн', '—'],
        ['Ответственный', '—'],
        ['Релиз исправления', '—'],
        ['Коммиты / MR', '—'],
    ]

    return (
        <article className="ticket-card">
            <header className="ticket-card-header">
                <h1>Тикет BUG-{ticket.id}</h1>

                <div className="ticket-card-actions">
                    <button type="button" disabled>Редактировать</button>
                    <button type="button" className="ticket-delete" disabled>
                        Удалить
                    </button>
                </div>
            </header>

            <div className="ticket-card-layout">
                <div>
                    <div className="ticket-card-heading">
                        <h2>{ticket.title}</h2>
                        <span className="ticket-status">{ticket.status}</span>
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

                <dl className="ticket-details">
                    {details.map(([label, value]) => (
                        <div key={label}>
                            <dt>{label}</dt>
                            <dd>{value}</dd>
                        </div>
                    ))}
                </dl>
            </div>
        </article>
    )
}