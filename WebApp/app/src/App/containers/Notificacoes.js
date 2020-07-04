import React from 'react';
import axios from 'axios';
import './Notificacoes.css';

class Notificacoes extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: "Bearer " + this.props.token,
            notifs: '',
        };

        this.printAllNotifs = this.printAllNotifs.bind(this);

    }

    componentDidMount() {
        this.loadNotifs();
    }

    async loadNotifs() {
        await axios.get('https://localhost:44301/' + this.props.accounttype + '/notificacoes', {
            params: {
                Username: this.props.user.username
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.setState({ notifs: response.data });
                //console.log(response.data);
            })
            .catch(response => {
                console.log(response);
            })
    }

    deleteNotif = (notifid) => {
        //alert("Deleting notif id: " + notifid);
        axios({
            method: 'delete',
            url: 'https://localhost:44301/' + this.props.accounttype + '/notificacoes/elim',
            data: JSON.stringify(this.props.username + '-|-' + notifid),
            headers: {
                "Content-Type": "application/json",
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                this.loadNotifs();
                this.props.updateNotifs();
            })
            .catch(response => {
                console.log(response);
                this.loadNotifs();
                alert("Erro a apagar a notificação.");
            })
    };

    printAllNotifs() {
        return (this.state.notifs.length > 0 ? this.state.notifs.map((notif, index) =>
            <div style={{ textAlign: "Left" }} className="list-group-item flex-column align-items-start">
                <div className="d-flex w-100 justify-content-between">
                    <p className="mb-1 notif-page-text">{notif.conteudo}</p>
                </div>
                <small class="text-muted trashdiv">{notif.dataEmissao.substring(11, 16) + " - " + notif.dataEmissao.substring(8, 10)
                    + "/" + notif.dataEmissao.substring(5, 7) + "/" + notif.dataEmissao.substring(0, 4)}</small>
                <svg onClick={() => this.deleteNotif(notif.id)} width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-trash-fill notiftrashcan" fill="grey" xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd" d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z" />
                </svg>
            </div>
        ) : <div><p className="notif-page-text">Não tem notificações a apresentar.</p></div>)
    }

    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-10">
                        <div className="card login-card">
                            <div className="card-block">
                                <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                <h5 className="card-text login-text">Notificações</h5>
                                <div className="list-group notifsdiv">
                                    {this.printAllNotifs()}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="col"></div>
                </div>
            </div>
        )
    }
}

export default Notificacoes;