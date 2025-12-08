# 🚀 DevLearning - Microservices Refactoring

> **De Monólito para Microserviços.**

![Build Status](https://img.shields.io/badge/Build-Passing-success)
![Platform](https://img.shields.io/badge/Platform-.NET%209-blueviolet)
![Architecture](https://img.shields.io/badge/Architecture-Microservices-blue)
![Database](https://img.shields.io/badge/Database-SQL%20%2B%20MongoDB-green)

## 📖 Sobre o Projeto

O **DevLearning** nasceu como um projeto monolítico de ensino (LMS). Este repositório documenta a jornada de refatoração completa para uma arquitetura de **Microsserviços**, focada em resolver problemas de escalabilidade e acoplamento.

O principal desafio foi migrar de um banco de dados relacional único para o padrão **Database-per-Service**, implementando **Persistência Poliglota** (SQL Server e MongoDB convivendo em harmonia).

---

## 🏗️ Nova Arquitetura

O sistema foi fatiado em **5 APIs independentes** baseadas em Domínios (Bounded Contexts):

| Serviço (API) | Porta | Banco de Dados | Responsabilidade |
| :--- | :--- | :--- | :--- |
| **Category API** | `5001` | SQL Server | Categorização de cursos. |
| **Author API** | `6001` | SQL Server | Gestão de Autores e Biografias. |
| **Course API** | `7001` | SQL Server | O Core do sistema. Dados rígidos e relacionais. |
| **Student API** | `8001` | **MongoDB** | Perfil do aluno, Histórico e Matrículas. |
| **Career API** | `9001` | **MongoDB** | Agregação de cursos em trilhas de carreira. |

---

<br>

<div align="center">

  <h3>🎓 Projeto Acadêmico</h3>
  <p>Proposto pelo <b>Prof. Felipe Pestana</b></p>

  <h3>🚀 Squad DevLearning</h3>
  <p>
    Este projeto foi refatorado e desenvolvido com ❤️ por:
  </p>
  
  <table>
    <tr>
      <td align="center">👩‍💻<br><b>Érica Gonçalves</b></td>
      <td align="center">👨‍💻<br><b>Everton Silva</b></td>
      <td align="center">👨‍💻<br><b>Felipe Sperli Neto</b></td>
      <td align="center">👩‍💻<br><b>Giovanna Fornazari</b></td>
      <td align="center">👩‍💻<br><b>Natalia Zamperlini</b></td>
    </tr>
  </table>

</div>

