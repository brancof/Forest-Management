import React from 'react';
import axios from 'axios';
import Maps from './Maps';
import './Proprietarios.css'
import { Switch, Route, Link } from 'react-router-dom';
import Options from './Options';
import Notificacoes from './Notificacoes';
import ProprietarioRelatorio from './ProprietarioRelatorio';

class Proprietarios extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            terrenos: '',
            mapInfo: [],
            auth: "Bearer " + this.props.token,
            checked: [], 
            terrenoSelected: null
        };

        this.loadTerrenos = this.loadTerrenos.bind(this);
        this.terrenostable = this.terrenostable.bind(this);
        this.handleChangeTerreno = this.handleChangeTerreno.bind(this);
        this.handleCheck = this.handleCheck.bind(this);

        this.loadTerrenos();
    }

    handleCheck(event) {
        this.state.checked[event.target.value] = event.target.checked;
        //this.forceUpdate();
    }

    terrenostable() {
        return (this.state.terrenos.length > 0 ? this.state.terrenos.map((terreno, index) =>
            <tr key={terreno.id_Terreno}>
                <td className="colmorada" style={{ textAlign: "left", paddingLeft: "5%" }}>
                    <input style={{ display: "inline", visibility: terreno.estadoLimpeza ? "hidden" : "visible" }} type="checkbox" disabled={terreno.estadoLimpeza} key={terreno.id_Terreno} onChange={this.handleCheck} value={index} className="form-check-input" id="checkmark" />
                    <Link to='/proprietarios/relatorio' onClick={this.handleClickLink.bind(this, terreno)}>
                        {terreno.morada} - {terreno.cod_postal}
                    </Link>
                </td>
                <td className="colestado" style={{ textAlign: "left" }}>{terreno.estadoLimpeza ? "Limpo" : "Não Limpo"}</td>
            </tr>
        ) : null)
    }

    handleClickLink(terreno){
        this.setState({terrenoSelected: terreno});
    }

    loadTerrenos() {
        //const auth = "Bearer " + this.props.token;
        axios.get('https://localhost:44301/proprietarios/terrenos', {
            params: {
                Username: this.props.username
            },
            headers: {
                "Authorization": this.state.auth
            }
        })
            .then(response => {
                //alert("Login efectuado com successo.");
                this.setState({ terrenos: response.data });
                console.log(response.data);
                response.data.map((terreno, index) => {
                    this.state.checked[index] = false;
                });
                var mapterr = [];
                response.data.map((terreno, index) => {
                    mapterr[index] = {
                        name: terreno.morada,
                        lat: terreno.latitude,
                        lng: terreno.longitude,
                        estado: terreno.estadoLimpeza
                    };
                });
                this.setState({ mapInfo: mapterr });

                //this.forceUpdate();
            })
            .catch(response => {
                alert("Erro no carregamento de terrenos.");
                console.log(response);
            })
    }

    handleChangeTerreno(event) {
        event.preventDefault();

        if (!this.state.terrenos.length > 0) return;
        var i;
        for (i = 0; i < this.state.checked.length; i++) {
            if (this.state.checked[i]) {
                axios({
                    method: 'put',
                    url: 'https://localhost:44301/proprietarios/limpeza',
                    data: JSON.stringify(this.props.username + ',' + this.state.terrenos[i].id_Terreno),
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": this.state.auth
                    }
                })
                    .then(response => {
                        console.log(response);
                        this.loadTerrenos();
                        //this.forceUpdate();
                    })
                    .catch(response => {
                        alert("Erro na limpeza do terreno.");
                        console.log(response);
                    })
            }
        }
    }


    render() {

        return (
            <Switch>
                <Route exact path='/proprietarios'>
                    <div className="container login-container">
                        <div className="row">
                            <div className="col"></div>
                            <div className="col-md-10">
                                <div className="card login-card">
                                    <div className="card-block">
                                        <h4 className="card-title login-title">{this.props.user.nome}</h4>
                                        <p className="card-text login-text">Gestão de Propriedades</p>
                                        <table className="table table-hover table-bordered table-prop">
                                            <thead>
                                                <tr>
                                                    <th scope="col colmorada">Morada</th>
                                                    <th scope="col colestado">Estado</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {this.terrenostable()}
                                            </tbody>
                                        </table>
                                        <input className="btn btn-success btn-sm btn-add-prop" type='submit' onClick={this.handleChangeTerreno} value="Alterar Estado" />
                                        <div className="map-container">
                                            <Maps mapInfo={this.state.mapInfo} />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className="col"></div>
                        </div>
                    </div>
                </Route>

                <Route path='/proprietarios/opcoes'>
                    <Options user={this.props.user} username={this.props.username} change={{ user: this.props.change }} token={this.props.token} accounttype={this.props.accounttype} />
                </Route>

                <Route path='/proprietarios/notificacoes'>
                    <Notificacoes user={this.props.user} username={this.props.username} change={{ user: this.props.change }} accounttype={this.props.accounttype} token={this.props.token} />
                </Route>

                <Route path='/proprietarios/relatorio'>
                    <ProprietarioRelatorio user={this.props.user} username={this.props.username} selected={this.state.terrenoSelected} token={this.props.token} />
                </Route>

            </Switch>
        );
    }
}

export default Proprietarios;